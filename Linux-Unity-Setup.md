# Installation of Unity and setting up IDE
1. [Install and open Unity Hub](https://docs.unity3d.com/hub/manual/InstallHub.html#install-hub-linux)
    1. Register and login
    2. Activate personal license(Preferences(cogwheel in the top right corner) -> Licenses)
    3. clone the repo with git and add it(https://github.com/kanitkameh/Game-Dev-Course-2022)

2. Download an IDE. [Visual studio is a good choice.](https://code.visualstudio.com/docs/?dv=linux64_deb)
3. [Download and install .NET.](https://dotnet.microsoft.com/en-us/download/dotnet/sdk-for-vs-code?utm_source=vs-code&amp;utm_medium=referral&amp;utm_campaign=sdk-install)
    After that add .NET to PATH: 
    ```
    export PATH="$PATH:$HOME/.dotnet"
    ```
    or
    ```
    sudo ln -s /home/kanitkameh/.dotnet/dotnet /bin/dotnet
    ```
# Troubleshooting: 
Try symlinks. If they don't work too, check your symlinks if they use relative instead of absolute paths.
