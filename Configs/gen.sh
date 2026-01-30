#!/bin/bash

# 设置 Luban.dll(mac) 的路径
#LUBAN_DLL=../Luban/Luban.dll

LUBAN_DLL=./tools/Luban/Luban.dll


# 设置 DataTables 的路径
#CONF_ROOT=../LubanData/luban.conf

CONF_ROOT=./source/luban.conf

# 设置 代码输出目录 的路径
CODE_OUTPUT=../Assets/Assemblies/GGJ2026/Runtime/Configs/Luban

# 设置 json数据输出目录 的路径
DATA_OUTPUT=../Assets/Assemblies/GGJ2026/Resource/Config/Luban
# 运行 Luban
dotnet "$LUBAN_DLL" \
    -t client \
    -c cs-simple-json \
    -d json \
    --conf "$CONF_ROOT" \
    -x outputCodeDir="$CODE_OUTPUT" \
    -x outputDataDir="$DATA_OUTPUT"