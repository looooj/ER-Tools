%~d0
cd %~p0
SetLocal

del /q /s tmp\*.*
cd tmp
rd /s /q ER-Tools
md ER-Tools
cd ER-Tools
copy ..\..\*.md 
copy ..\..\ERBackup\bin\Release\net6.0-windows\*.* 
copy ..\..\ERParamEditor\bin\Release\net6.0-windows\*.* 
del *.txt
del *.pdb
md assets
xcopy /s /y /q ..\..\assets  assets\
md docs 
xcopy /s /y /q ..\..\docs  docs\

md templates
xcopy /s /y /q ..\..\templates  templates\

md projects


