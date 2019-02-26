main: compile exec
compile:
	@xbuild ArchBench.sln
exec:
	@mono ./ArchBench.Server/bin/Debug/ArchBench.Server.exe