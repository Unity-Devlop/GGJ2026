dotnet ./tools/Luban/Luban.dll ^
    -t client ^
    -c cs-simple-json ^
    -d json  ^
    --conf ./source/luban.conf ^
    -x outputCodeDir=../Assets/Assemblies/Jump/Runtime/Configs/Luban ^
    -x outputDataDir=../Assets/Assemblies/Jump/Resource/Config/Luban

pause

