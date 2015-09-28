
# NiteLiteServer

Small, lightweight API server based on OWIN. This server is meant to run on smaller devices (like the Raspberry PI). The only thing that has to be supported is the latest release of Mono. 

## Base Features

* Lightweight, only using about 80MB of memory with an approximate load of 2000 users on a Raspberry PI 2 running Raspbian and Mono
* SignalR Support (real-time messaging using Websockets)
* Web API Support (create your own controllers in separate assemblies and load only what you need)
* NLog Support 
* CORS Support
* Static Files Support (Deliver static websites using NiteLite)
* Web API Attribute based routing
* Each feature can be separately enabled or disabled using settings.conf
* Configuration is JSON based
* For each Web API Assembly, you simply add a separate configuration file for your settings
* Basic Authentication support (not completed yet, but next on list) 
* BCrypt encryption for passwords
* Database support to save sensor data quickly and efficiently under Windows and Linux (next on list as well)

## How to install NiteLite

### Installing the latest Mono release on your Raspberry Pi 2

The first thing you have to do, is to install Mono on your Raspberry PI 2, here is an excellent link on how to accomplish this task:

[Getting started with the Raspberry Pi 2, for .NET developers](http://j.tlns.be/2015/02/04/getting-started-with-the-raspberry-pi-2-for-net-developers/ "Jahn Tielens Blog")

### Binary deployment

After you have finished this step, simply download the latest binary release and copy that to your Raspberry PI, using SCP or you can create a Windows Share using Samba. In either case, here are links how to use SCP and how to create a SAMBA Windows Share on your Raspberry PI, running Raspbian:

### Deploying the binaries to your Raspberry PI 2 using WinSCP or SAMBA

* For SCP functionality on Windows, download [WinSCP](https://winscp.net/eng/download.php)
* For a tutorial on how to install SAMBA and configure a Windows Share on your Raspberry PI, use this tutorial:  [How-To: Share a folder with a Windows computer from a Raspberry Pi](http://raspberrypihq.com/how-to-share-a-folder-with-a-windows-computer-from-a-raspberry-pi/)

### Deploying from source

Simply clone the repository, build the project and copy the files from the output directory to your Raspberry PI. Basically the same procedure as you would use with the binary release you can download from GitHub.

## How to configure NiteLite

Within the root-directory of the binary-distribution, you will find two configuration files:

* default.conf
* settings.conf

The first of the two files, is used to represent the basic, or default settings for NiteLite. The second file (which you should use to configure NiteLite), will be merged with the default file and is meant to contain your settings.

So, if you omit a specific setting in settings.conf, NiteLite will use the setting from default.conf. It's simple as that. Here is what the settings.conf file looks like:

```json
{
      
    EnableNLog: true,
    EnableCors: true,
    EnableSignalR: true,
    EnableAttributeRouting: true,
	EnableStaticFiles: true
    
}
```

This file should be pretty self-explaining. Anything you don't want to be active, set it to false otherwise true. Simple as that.

## How to start NiteLite

NiteLite accepts two command-line parameters:

* The port it will listen on, e.g. port 80
* And the host-name or IP-Address, e.g raspberry, 192.168.25.5

To start NiteLite, simply go into the root-directory of your binary NiteLite deployment and type

```bash
mono NiteLIteServer.exe -p 8080 -h 192.168.25.5
```

If everything goes well (port is not in use for example), NiteLite will start-up and your server is ready to use. To Browse the static HTML content, use the following URL:

http://[your ip or hostname]/www

This will open the basic bootstrap static page on your NiteLite server.

## Replacing or editing the static content served by NiteLite

Within your deployment root, you will find a folder called "www". Edit the files within this folder to customize index.html to suite your needs. The default document is always called "index.html". You can also replace this folder with your own HTML template or page, as long as the default html file is called "index.html".



## License

The MIT License (MIT)

Copyright (c) 2015, Ilija Injac, aka @awsomedevsigner

Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.







