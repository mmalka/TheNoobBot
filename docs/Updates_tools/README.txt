How-To : Get the functions name retranscripted in new WoW binary IDB file. 

Installations :
Copy \Plugins IDA\patchdiff2.plw to \IDA_DIR\Plugins\

First of all, load the NEWEST build in IDA as a new PE. (important!)
Use Ctrl+F8 to load "Patchdiff2" and then load the OLD build IDB with your functions renamed in.

It will output you the list of "matched_functions" and "identical_functions", 
now, the rest of the guides will need do be done for both matched and identical.

Right clic in the output, then click on "Copy All".

Paste the output in a .txt file anywhere on your computer.

Compile PD2Trimmer, and launch the program, click on "Clean me up !",
then load the .txt you just created.

Copy/Paste the output of the software in the .txt (replace old with trimmed).

Open the IDC script \Script IDA\RenameFunctions.idc, it will ask you to load a file,
then load the .txt with the trimmed output of PatchDiff2.

It should have replaced all the functions name, now, just open the IDC script
\Script IDA\LabelLuaFuncs.idc and \Script IDA\LabelCvars.idc.

Save your IDB file, and you there you go :)