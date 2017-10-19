/*
 * Copyright (c) 1995, 2000, 2014-2017 
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cryptography.PolyCrypt.CryptoCraftConsole.Utility
{
    /// <summary>
    ///     I am a simple run argument parser.
    ///     o I can parse one type of parameters, those that start with a minus '-' sign. (Unix style)
    ///     o I can accept parameters with an '=' modifier.
    ///     o I can verify required parameters exist.
    ///     o I will reject any parameters that are neither required nor optional.
    /// </summary>
    public class ParseRunArguments
    {
        private readonly List<string> _errorMessages = new List<string>();

        /// <summary>
        ///     Result dictionary of key-value-pair's that contains all of the valid parameters.  Dictionary key is the lower case
        ///     value of the parameters name or 'key'.  The parameters name is stored in the kvp as its entered case.
        /// </summary>
        private readonly Dictionary<string, KeyValuePair<string, string>> _parameters =
            new Dictionary<string, KeyValuePair<string, string>>();

        /// <summary>
        ///     Parse an array of arguments and verify that required parameters exist.
        ///     Errors may be tested for by checking the HasErrors attribute.
        /// </summary>
        /// <param name="args">An array of string arguments, typically from the main() method of a program.</param>
        /// <param name="requiredParameters">
        ///     A list of  required parameters separated by '|'; i.e.
        ///     "Server=XXX|Catalog=yyy|Login=zzz"
        /// </param>
        /// <param name="optionalParameters">A list of optional parameters separated by '|'; i.e. "Password=zzz|debug=yyy</param>
        public ParseRunArguments(IEnumerable<string> args, string requiredParameters, string optionalParameters)
        {
            // All arguments start with either a - sign.
            // the final argument may be a single work such as a filename.

            var scanParams = ScanParameters(requiredParameters, optionalParameters);

            var validArgument =
                new Regex($@"^-(({scanParams})|(\w+)=([\w:_/\.\\\' ]+))$",
                    RegexOptions.IgnoreCase);

            var nullError = false;
            foreach (var a in args)
            {
                if (string.IsNullOrEmpty(a)) // only write null param message once.
                {
                    if (!nullError) _errorMessages.Add("A parameter was null or empty");
                    nullError = true;
                }
                else
                {
                    // is it a valid argument? -name=param || last argument may be a filename.
                    // TODO: Last argument may be a filename feature
                    var m = validArgument.Match(a);
                    if (!m.Success)
                    {
                        _errorMessages.Add($"'{a}' is not a valid argument");
                    }
                    else
                    {
                        // require default kvp for catch block below
                        var kvp = new KeyValuePair<string, string>("other error", string.Empty);

                        try
                        {
                            if (string.IsNullOrEmpty(m.Groups[2].Value))
                            {
                                kvp = new KeyValuePair<string, string>(m.Groups[3].Value, m.Groups[4].Value);
                                _parameters.Add(kvp.Key.ToLower(CultureInfo.CurrentCulture), kvp);
                            }
                            else
                            {
                                kvp = new KeyValuePair<string, string>(m.Groups[2].Value, string.Empty);
                                _parameters.Add(kvp.Key.ToLower(CultureInfo.CurrentCulture), kvp);
                            }
                        }
                        catch (ArgumentException)
                        {
                            _errorMessages.Add($"Duplicate parameter: '{kvp.Key}'");
                        }
                    }
                }
            }

            // Verify required parameters are present
            if (!string.IsNullOrEmpty(requiredParameters))
                foreach (var required in requiredParameters.Split('|'))
                    if (!ContainsKey(required.ToLower(CultureInfo.CurrentCulture)))
                        _errorMessages.Add($"Missing required argument '-{required}'");
        }

        /// <inheritdoc />
        /// <summary>
        ///     Accept all arguments as valid.  Arguments must be valid alpha numeric characters and can not include white space.
        ///     Duplicate arguments are rejected and flagged as errors.
        /// </summary>
        /// <param name="args"></param>
        public ParseRunArguments(IEnumerable<string> args) : this(args, string.Empty, @"\w+")
        {
        }

        /// <summary>
        ///     Allow this class to be extended with no arguments passed to generate an instance.
        /// </summary>
        /// <remarks>Used exclusively by extensions to this class (such as the NUnit test for protected members)</remarks>
        protected ParseRunArguments()
        {
        }

        public KeyValuePair<string, string> this[string key] => Parameters[key];

        /// <summary>
        ///     Total number of parameters.
        /// </summary>
        public int Count => Parameters.Count;

        /// <summary>
        ///     True if duplicate/missing/unknown parameter errors were found in parameters
        /// </summary>
        public bool HasErrors => _errorMessages.Count > 0;

        public Dictionary<string, KeyValuePair<string, string>> Parameters => _parameters;

        /// <summary>
        ///     Verifies that a parameter with given name exists.  Name is case insensitive.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsKey(string name)
        {
            return Parameters.ContainsKey(name.ToLower(CultureInfo.CurrentCulture));
        }

        /// <summary>
        ///     Get the parameter specified by the parameters name. Name is case insensitive.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public KeyValuePair<string, string> Get(string name)
        {
            return Parameters[name.ToLower(CultureInfo.CurrentCulture)];
        }

        /// <summary>
        ///     Concat required and optional regex strings with a "|".
        /// </summary>
        /// <param name="required">set of Param names of the format "parm1|parm2|parm3". May be null or String.Empty</param>
        /// <param name="optional">set of Param names of the format "parm1|parm2|parm3". May be null or String.Empty</param>
        /// <returns>
        ///     Returns required and optional contatenated by an "|". If no result returns 'String.Empty'. Never returns
        ///     'null'
        /// </returns>
        protected string ScanParameters(string required, string optional)
        {
            // if no optional params then required 
            // else if no required then optional 
            // else join them with an or ('|')
            var result = string.IsNullOrEmpty(optional)
                ? required
                : string.IsNullOrEmpty(required)
                    ? optional
                    : string.Join("|", required, optional);

            return string.IsNullOrEmpty(result) ? string.Empty : result;
        }

        /// <summary>
        ///     Array of messages indicating the error found.  In order of error occurrence.
        /// </summary>
        public string[] ErrorMessages()
        {
            return _errorMessages.ToArray();
        }

        public override string ToString()
        {
            var items = new List<string> { base.ToString() };

            items.AddRange(Parameters.Select(item => item.Value).Select(paramItem =>
                $"'{paramItem.Key}'='{paramItem.Value}'"));

            items.AddRange(ErrorMessages());


            return string.Join("\n", items.ToArray());
        }
    }
}