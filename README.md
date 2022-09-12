# DXLog.net Custom Forms

*DXLog.net* offers a unique customization ability in that you can design your own "subwindows"
also referred to as *Forms*. A Form can contain information, data or metrics that you do not
get from one of the standard *DXLog.net* windows. On the *DXLog.net* web site you can download
example code for a single custom form.

A custom form is compiled into a DLL file and "installed" by placing it in a subfolder 
to *DXLog.net's* installation folder.
Contrary to *DXLog.net* script files, this means you have to compile your code into an executable
binary.

Some (hard earned) advice for designing custom Forms in *DXLog.net*:

To write, modify and compile custom Foms you need to install *Visual Studio*. The "*Community*" version
is free for personal use. The example code should be opened through its solutions file (.sln).
If you can not see the code, right click on the Form and select "view code"

Just as with the scripts, you need to add proper references to relevant parts of the *DXLog.net*
binaries such as *DXLog.net.exe*, *DXLogDAL.dll* etc. Without this, *Visual Studio Intellisense*
will complain about e.g. variable names not being available and you will not be able to build.

Make sure *Visual Studio* is set up to build for x86. (Click on the selection arrow where it says
"any CPU" and change to "x86".) The shortcut key to building the binary is Shift-Ctrl-B.
If it does not already exist, create a folder called "CustomForms" in the *DXLog.net*
installation folder (typically *C:\Program Files (x86)\DXLog.net*)
Copy the created DLL file (located in *DXLCusFrm1\bin\x86\Debug* or similar, depending on your
settings.) to the *CustomForms* folder.

Start *DXLog.net* and find a new drop-down menu called "*Custom*" between "*Windows*" and "*Help*".  
Use it to activate your new custom Form. You can contain as many custom Forms as you like
in a single DLL but they all must

* Have different names (i.e. the name of the *KForm* object)
* Have different FormsID (change both in the *CusFormID* method for the class and in the *Forms* property)

For more information see www.sm7iun.se
