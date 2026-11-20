rem c_bakbnd

set xbnd=%out_dir%/%1.talkesdbnd.dcx
if not exist "%xbnd%.bak" (
    copy %xbnd% %xbnd%.bak
)
copy /y %xbnd%.bak %xbnd%


rem c_std_env

set base_dir=F:\docs\unpack117b
rem set mod_dir=E:\games\er\st\mod
set out_dir=E:\games\er\st\mod\script\talk
set rp_dir=D:\myprojects\game-tools\ER-Tools\commons\esd

rem c_mod1

if "x%1" == "x" goto xerror
:xstart
esdtool -er -basedir %base_dir% -f %1 -writepy %1.py
ReplaceTool %1.py %rp_dir%\%2
goto xend
:xerror
echo "need py file name"
:xend

rem c_mod2

if "x%1" == "x" goto xerror
set xfiles=
:xcheck_arg
set xfiles=%xfiles% %1.py.py   
shift
if "x%1" == "x" goto xstart
goto xcheck_arg
:xstart
esdtool -er -basedir %base_dir% -i %xfiles%   -writebnd %out_dir%

goto xend
:xerror
echo "need py file name"
:xend

rem c_std2

setlocal
call c_std_env.bat

call c-mod1 t602006000 rp02.txt
call c-mod1 t800006000 rp04.txt
call c-mod2 t602006000 t800006000

call c-mod1 t000001000 rp01.txt
call c-mod2 t000001000

