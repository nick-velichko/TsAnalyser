﻿#Cinegy TS Analyser Tool

Use this tool to view inbound network, RTP and TS packet details. Use newly introduce powers to view into the service description tables, and even decode a teletext stream!

##How easy is it?

Well, we've added everything you need into a single teeny-tiny EXE again, which just depends on .NET 4. And then we gave it all a nice Apache license, so you can tinker and throw the tool wherever you need to on the planet.

Just run the EXE from inside a command-prompt, and a handy help message will pop up like this:

Once you run the EXE, if you enabled the embedded web service, you can browse to:

http://localhost:8124/index.html 

And see some realtime things displayed in your browser (great if you run the analyser headless on remote machines).

##Command line arguments:

Double click, or just run without (or with incorrect) arguments, and you'll see this:

```
Cinegy Transport Stream Monitoring and Analysis tool v1.1.0 (26/09/2016 19:23:56)

TsAnalyser 1.1.0.0
Copyright © Cinegy GmbH 2016

ERROR(S):
  -m/--multicastaddress required option is missing.
  -g/--mulicastgroup required option is missing.


  -q, --quiet                  (Default: False) Don't print anything to the
                               console

  -m, --multicastaddress       Required. Input multicast address to read from.

  -g, --mulicastgroup          Required. Input multicast group port to read
                               from.

  -l, --logfile                Optional file to record events to.

  -a, --adapter                IP address of the adapter to listen for
                               multicasts (if not set, tries first binding
                               adapter).

  -w, --webservices            (Default: False) Enable Web Services (available
                               on http://localhost:8124/ by default).

  -u, --serviceurl             (Default: http://localhost:8124/) Optional
                               service URL for REST web services (must change
                               if running multiple instances with web services
                               enabled.

  -s, --servicedescriptions    (Default: False) Optional instruction to decode
                               further DVB service description metadata
                               (experimental)

  -n, --nortpheaders           (Default: False) Optional instruction to skip
                               the expected 12 byte RTP headers (meaning plain
                               MPEGTS inside UDP is expected

  --help                       Display this help screen.


```

Because most of the time you might want a quick-and-dirty scan of a stream, if you just double-click the EXE (or run without arguments) it will ask you interactively what multicast address and group you want to listen to - perfect for people that hate typing!

Even better, just to make your life easier, we auto-build this using AppVeyor - here is how we are doing right now: 

[![Build status](https://ci.appveyor.com/api/projects/status/08dqscip26lr0g1o/branch/master?svg=true)](https://ci.appveyor.com/project/cinegy/tsanalyser/branch/master)

We're just getting started, but if you want you can check out a compiled binary from the latest code here:

[AppVeyor TSAnalyser Project Builder](https://ci.appveyor.com/project/cinegy/tsanalyser/build/artifacts)

