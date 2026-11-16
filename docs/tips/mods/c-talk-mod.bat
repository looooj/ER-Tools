if "x%1" == "x" goto xerror
:xstart
esdtool -er -basedir %base_dir%  -f %1 -writepy %1.py
ReplaceTool %1.py D:\myprojects\game-tools\ER-Tools\templates\commons\%2
esdtool -er -basedir %base_dir%  -i %1.py.py -writebnd %out_dir%
goto xend
:xerror
echo "need py file name"
:xend

