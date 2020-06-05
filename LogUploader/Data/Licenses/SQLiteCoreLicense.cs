﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.Licenses
{
    class SQLiteCoreLicense : ISoftwareLicense
    {
        public string Product { get => "System.Data.SQLite.Core"; }
        public string Owner { get => "SQLite Development Team"; }
        public string Type { get => "Public Domain"; }
        public string Text { get => @"SQLite Is Public Domain

All of the code and documentation in SQLite has been dedicated to the public domain by the authors. All code authors, and representatives of the companies they work for, have signed affidavits dedicating their contributions to the public domain and originals of those signed affidavits are stored in a firesafe at the main offices of Hwaci. Anyone is free to copy, modify, publish, use, compile, sell, or distribute the original SQLite code, either in source code form or as a compiled binary, for any purpose, commercial or non-commercial, and by any means.

The previous paragraph applies to the deliverable code and documentation in SQLite - those parts of the SQLite library that you actually bundle and ship with a larger application. Some scripts used as part of the build process (for example the ""configure"" scripts generated by autoconf) might fall under other open-source licenses. Nothing from these build scripts ever reaches the final deliverable SQLite library, however, and so the licenses associated with those scripts should not be a factor in assessing your rights to copy and use the SQLite library.

All of the deliverable code in SQLite has been written from scratch. No code has been taken from other projects or from the open internet. Every line of code can be traced back to its original author, and all of those authors have public domain dedications on file.So the SQLite code base is clean and is uncontaminated with licensed code from other projects.
Open-Source, not Open-Contribution

SQLite is open-source, meaning that you can make as many copies of it as you want and do whatever you want with those copies, without limitation. But SQLite is not open-contribution.In order to keep SQLite in the public domain and ensure that the code does not become contaminated with proprietary or licensed content, the project does not accept patches from unknown persons.

All of the code in SQLite is original, having been written specifically for use by SQLite.No code has been copied from unknown sources on the internet.
Warranty of Title

SQLite is in the public domain and does not require a license.Even so, some organizations want legal proof of their right to use SQLite.Circumstances where this occurs include the following:


   Your company desires indemnity against claims of copyright infringement.
   You are using SQLite in a jurisdiction that does not recognize the public domain.
   You are using SQLite in a jurisdiction that does not recognize the right of an author to dedicate their work to the public domain.
   You want to hold a tangible legal document as evidence that you have the legal right to use and distribute SQLite.
   Your legal department tells you that you have to purchase a license.

If any of the above circumstances apply to you, Hwaci, the company that employs all the developers of SQLite, will sell you a Warranty of Title for SQLite.A Warranty of Title is a legal document that asserts that the claimed authors of SQLite are the true authors, and that the authors have the legal right to dedicate the SQLite to the public domain, and that Hwaci will vigorously defend against challenges to those claims.All proceeds from the sale of SQLite Warranties of Title are used to fund continuing improvement and support of SQLite.
Contributed Code

In order to keep SQLite completely free and unencumbered by copyright, the project does not accept patches. If you would like to make a suggested change, and include a patch as a proof-of-concept, that would be great. However please do not be offended if we rewrite your patch from scratch. "; }
    }
}
