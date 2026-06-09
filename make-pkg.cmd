%~d0
cd %~p0
SetLocal

del /q /s tmp\*.*
cd tmp
rd /s /q ER-Tools
md ER-Tools
cd ER-Tools
copy ..\..\*.md 
copy ..\..\ERBackup\bin\Release\net8.0-windows7.0\*.* 
xcopy /s /y /i /q ..\..\ERParamEditor\bin\Release\net8.0-windows7.0\*.* 
del *.txt
del *.pdb
md assets
xcopy /s /y /i /q ..\..\assets  assets\
md docs 
xcopy /s /y /i /q ..\..\docs  docs\
copy docs\default-config.txt config.txt
copy docs\default-param-names.txt  param-names.txt
md templates
xcopy /s /y /i /q ..\..\templates  templates\

md projects


