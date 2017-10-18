find . -type f|egrep -v '\/bin|\/obj|.user$|.vspscc$|\.vs|\.git'|tar -cvz --file=../archive.tar.gz --files-from=-
