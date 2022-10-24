@echo off
rem get the location of the git executable from
rem %localappdata%\GithubDesktop\app*\resources\app\git\cmd\git.exe

set gitpath=%localappdata%\GithubDesktop\
for /f "delims=" %%a in ('dir /b /ad %gitpath%\app*') do set gitpath=%gitpath%%a\resources\app\git\cmd\git.exe
echo "git should be at %gitpath%"

%gitpath% config core.hooksPath .githooks

echo "Git Hooks installed :)"
