all:
	dotnet watch run

clean:
	dotnet clean
	rm -rf bin/ obj/ logs/ Servindustria.sln