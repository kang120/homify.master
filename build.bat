set /p token=<../_github_token.txt
docker build --no-cache --build-arg PACKAGE_TOKEN=%token% -t masterapi .
pause