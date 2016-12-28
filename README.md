# ProPsync Client installation instructions

## Client environment:
ProPsync is currently only available for Windows machines running ProPresenter 6.  Mac clients and versions supporting ProPresenter 5 will be released shortly.
ProPsync will need git installed.  On Windows, it will also need a number of Unix tools installed and these tools as well as Git executable from the command line.

## Git installation:
If the client setup is run before Git is installed, it will download and run the Git installer for you (it will take a few minutes to download though - during which time it will appear that nothing is happening.  Please follow these setup directions closely and if you already have git insatlled, consider reinstalling with these directions:

1\.  Agree to the EULA.  Keep in mind that Git is distrubuted under the GNUv2 license, whereas ProPsync is distributed under the MIT license.

![](https://downloads.semrauconsulting.com/propsync/readme-images/gitinstall/1-EULA.png "Git EULA")

2\.  Confirm the location to install Git in - C:\Program Files\Git.

![Ensure Git installs in C:\Program Files\Git](https://downloads.semrauconsulting.com/propsync/readme-images/gitinstall/2-location.png "Git install location")

3\.  Confirm the components.  Defaults are fine; however, you may take out the Windows Explorer integration if you wish.  Please leave the association settings as default though.

![Accept default compoents](https://downloads.semrauconsulting.com/propsync/readme-images/gitinstall/3-options.png "Git components")

4\.  The Start Menu step doesn't matter to ProPsync - choose your personal preference.

![Accept default start menu](https://downloads.semrauconsulting.com/propsync/readme-images/gitinstall/4-start_menu.png "Git start menu folder")

5\.  **THIS STEP IS EXTREMLY IMPORTANT** Select the last option ("Use Git and optional Unix tools from the Windows Command Prompt")

![Use both tools from the Windows Command Prompt](https://downloads.semrauconsulting.com/propsync/readme-images/gitinstall/5-git+unix_tools_options.png "Important step")

6\.  **This step is also very important** Select the last option ("Checkout as-is, commit as-is").  This prevents git from changing the contents of the files.

![Choose the option to leave files as-is.](https://downloads.semrauconsulting.com/propsync/readme-images/gitinstall/6-checkout_options.png "Important step")

7\.  **This step is also very important** Select the last option ("Use Windows' default console window").

![Choose the option to use the Windows default console window.](https://downloads.semrauconsulting.com/propsync/readme-images/gitinstall/7-terminal_options.png "Important step")

8\.  *This step is mildly important*.  Please de-select the "Enable Git Credential Manager" unless you plan to use it for another project.  While it hasn't interfered in our tests, the Git Credential Manager does have the potential to affect how our authentication mechanisms work.

![De-select the Git Credential Manager unless you plan on using it elsewhere](https://downloads.semrauconsulting.com/propsync/readme-images/gitinstall/8-operation_settings.png "Mildly important step")

9\.  We do not use the difftool (at least not yet); however, we recommend accepting the default to not enable it since it is experimental.

![Accept default for difftool](https://downloads.semrauconsulting.com/propsync/readme-images/gitinstall/9-difftool.png "Difftool settings")

10\. Wait for the install process to finish and the screen below will appear.  You do not need to launch Git Bash or view the release notes unless you want to.  You can now re-start the ProPsync client installation.

![Git installation completed](https://downloads.semrauconsulting.com/propsync/readme-images/gitinstall/10-finished.png "Setup completed")


## Client installation:
Be sure to extract the entire zip folder before running the "CoreInstall.exe" program.  Once the client installer detects that Git has been installed, it will allow you to perform the actual installation.  Be sure to have your server IP or DNS name handy as well as your username and password.

1\.  Wait for the splash screen to disappear, then agree to the EULA and enter your server address.




2\.  Click "Get Configuration", then enter your username and password that you setup during the server installation.  Full name and email address are also required for commit tracking and for Git configuration.  We highly recommending entering a identifying name for the person or group who will be using ProPresenter on this computer as it will come in handy if you ever need to revert to a previous version of a file.

  - You can also choose what repositories you want to sync as well as whether or not the program should use "Auto mode".
  - In "Auto mode", the program assumes you will start it instead of ProPresenter and you will let it open ProPresenter for you.  When it opens, it will do an initial sync, open ProPresenter, wait for ProPresenter to close, then sync again.  This is useful on machines that aren't presenting, but it is not advisable to use a method like this on mission critical presentation computers.  When it is not using "Auto mode", you will manually start the program and use the sync button to start a sync.  It would be advisible to do a sync before and after you open ProPresenter - and not using it while ProPresenter is open.
  
  
  
3\.  Once you start the installation, a window will pop up asking you to confirm the paths of each ProPresenter folder.  These paths will auto-fill if the default paths exist, but you should probably verify these paths by opening ProPresenter and looking at the paths listed in the preferences.  Once you have verified the paths, click "Confirm".



4\.  A command window will probably pop up asking you to confirm the authenticity of your server.  Type yes and press enter.

  - A number of command windows will pop up as the initial sync occurs.  *If you are prompted by a password on any of these windows*, forcibly end the installation from Task Manager and start over, because something happened and the SSH authentication mechanism did not get setup properly.  You could simply enter the password for your user that you specified in our installation window; however, you would need to do that each time you ran a sync for each folder that gets synced.  You do not want to do that... it gets old really fast.

  
  
5\.  Once the installation has completed, you should see a message box stating so and you should see a shortcut on the desktop for "ProPsync"