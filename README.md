# ProPsync Client installation instructions

## Client environment:
ProPsync is currently only available for Windows machines running ProPresenter 6.  Mac clients and versions supporting ProPresenter 5 will be released shortly.
ProPsync will need git installed.  On Windows, it will also need a number of Unix tools installed and these tools as well as Git executable from the command line.

## Git installation:
If the client setup is run before Git is installed, it will download and run the Git installer for you.  Please follow these setup directions closely and if you already have git insatlled, consider reinstalling with these directions:

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