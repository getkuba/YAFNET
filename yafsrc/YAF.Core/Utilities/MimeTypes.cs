﻿/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2013 Jaben Cargman
* Copyright (C) 2014-2021 Ingo Herbote
 * https://www.yetanotherforum.net/
 * 
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at

 * https://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

namespace YAF.Core.Utilities
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Based on a solution by
    /// https://forums.asp.net/t/1964610.aspx?How+to+check+the+uploaded+file+is+SECURE+
    /// The mime types.
    /// </summary>
    public class MimeTypes
    {
        /// <summary>
        /// The known types.
        /// </summary>
        private static List<string> knownTypes;

        /// <summary>
        /// The mime types.
        /// </summary>
        private static Dictionary<string, string> mimeTypes;

        /// <summary>
        /// Check if File Content Type is correct
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool FileMatchContentType(HttpPostedFile file)
        {
            if (knownTypes == null || mimeTypes == null)
            {
                InitializeMimeTypeLists();
            }

            var extension = Path.GetExtension(file.FileName).Replace(".", string.Empty).ToLower();

            var mime = mimeTypes.FirstOrDefault(m => m.Key == extension);

            return mime.Value == file.ContentType;
        }

        /// <summary>
        /// Check if File Content Type is correct
        /// </summary>
        /// <param name="fileName">
        /// The file Name.
        /// </param>
        /// <param name="contentType">
        /// The content Type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool FileMatchContentType(string fileName, string contentType)
        {
            if (knownTypes == null || mimeTypes == null)
            {
                InitializeMimeTypeLists();
            }

            var extension = Path.GetExtension(fileName).Replace(".", string.Empty).ToLower();

            var mime = mimeTypes.FirstOrDefault(m => m.Key == extension);

            return mime.Value == contentType;
        }

        /// <summary>
        /// The initialize mime type lists.
        /// </summary>
        private static void InitializeMimeTypeLists()
        {
            knownTypes = new[]
            {
                "text/plain", "text/html", "text/xml", "text/richtext", "text/scriptlet", "audio/x-aiff",
                "audio/basic", "audio/mid", "audio/wav", "image/gif", "image/jpeg", "image/pjpeg", "image/png",
                "image/x-png", "image/tiff", "image/bmp", "image/x-xbitmap", "image/x-jg", "image/x-emf",
                "image/x-wmf", "video/avi", "video/mpeg", "application/octet-stream", "application/postscript",
                "application/base64", "application/macbinhex40", "application/pdf", "application/xml",
                "application/atom+xml", "application/rss+xml", "application/x-compressed",
                "application/x-zip-compressed", "application/x-gzip-compressed", "application/java",
                "application/x-msdownload"
            }.ToList();

            mimeTypes = new Dictionary<string, string>
            {
                { "3dm", "x-world/x-3dmf" },
                { "3dmf", "x-world/x-3dmf" },
                { "a", "application/octet-stream" },
                { "aab", "application/x-authorware-bin" },
                { "aam", "application/x-authorware-map" },
                { "aas", "application/x-authorware-seg" },
                { "abc", "text/vnd.abc" },
                { "acgi", "text/html" },
                { "afl", "video/animaflex" },
                { "ai", "application/postscript" },
                { "aif", "audio/aiff" },
                { "aifc", "audio/aiff" },
                { "aiff", "audio/aiff" },
                { "aim", "application/x-aim" },
                { "aip", "text/x-audiosoft-intra" },
                { "ani", "application/x-navi-animation" },
                { "aos", "application/x-nokia-9000-communicator-add-on-software" },
                { "aps", "application/mime" },
                { "arc", "application/octet-stream" },
                { "arj", "application/arj" },
                { "art", "image/x-jg" },
                { "asf", "video/x-ms-asf" },
                { "asm", "text/x-asm" },
                { "asp", "text/asp" },
                { "asx", "application/x-mplayer2" },
                { "au", "audio/basic" },
                { "avi", "video/avi" },
                { "avs", "video/avs-video" },
                { "bcpio", "application/x-bcpio" },
                { "bin", "application/octet-stream" },
                { "bm", "image/bmp" },
                { "bmp", "image/bmp" },
                { "boo", "application/book" },
                { "book", "application/book" },
                { "boz", "application/x-bzip2" },
                { "bsh", "application/x-bsh" },
                { "bz", "application/x-bzip" },
                { "bz2", "application/x-bzip2" },
                { "c", "text/plain" },
                { "c++", "text/plain" },
                { "cat", "application/vnd.ms-pki.seccat" },
                { "cc", "text/plain" },
                { "ccad", "application/clariscad" },
                { "cco", "application/x-cocoa" },
                { "cdf", "application/cdf" },
                { "cer", "application/pkix-cert" },
                { "cha", "application/x-chat" },
                { "chat", "application/x-chat" },
                { "class", "application/java" },
                { "com", "application/octet-stream" },
                { "conf", "text/plain" },
                { "cpio", "application/x-cpio" },
                { "cpp", "text/x-c" },
                { "cpt", "application/x-cpt" },
                { "crl", "application/pkcs-crl" },
                { "css", "text/css" },
                { "def", "text/plain" },
                { "der", "application/x-x509-ca-cert" },
                { "dif", "video/x-dv" },
                { "dir", "application/x-director" },
                { "dl", "video/dl" },
                { "doc", "application/msword" },
                { "dot", "application/msword" },
                { "dp", "application/commonground" },
                { "drw", "application/drafting" },
                { "dump", "application/octet-stream" },
                { "dv", "video/x-dv" },
                { "dvi", "application/x-dvi" },
                { "dwf", "drawing/x-dwf (old)" },
                { "dwg", "application/acad" },
                { "dxf", "application/dxf" },
                { "eps", "application/postscript" },
                { "es", "application/x-esrehber" },
                { "etx", "text/x-setext" },
                { "evy", "application/envoy" },
                { "exe", "application/octet-stream" },
                { "f", "text/plain" },
                { "f90", "text/x-fortran" },
                { "fdf", "application/vnd.fdf" },
                { "fif", "image/fif" },
                { "fli", "video/fli" },
                { "flv", "video/x-flv" },
                { "for", "text/x-fortran" },
                { "fpx", "image/vnd.fpx" },
                { "g", "text/plain" },
                { "g3", "image/g3fax" },
                { "gif", "image/gif" },
                { "gl", "video/gl" },
                { "gsd", "audio/x-gsm" },
                { "gtar", "application/x-gtar" },
                { "gz", "application/x-compressed" },
                { "h", "text/plain" },
                { "help", "application/x-helpfile" },
                { "hgl", "application/vnd.hp-hpgl" },
                { "hh", "text/plain" },
                { "hlp", "application/x-winhelp" },
                { "htc", "text/x-component" },
                { "htm", "text/html" },
                { "html", "text/html" },
                { "htmls", "text/html" },
                { "htt", "text/webviewhtml" },
                { "htx", "text/html" },
                { "ice", "x-conference/x-cooltalk" },
                { "ico", "image/x-icon" },
                { "idc", "text/plain" },
                { "ief", "image/ief" },
                { "iefs", "image/ief" },
                { "iges", "application/iges" },
                { "igs", "application/iges" },
                { "ima", "application/x-ima" },
                { "imap", "application/x-httpd-imap" },
                { "inf", "application/inf" },
                { "ins", "application/x-internett-signup" },
                { "ip", "application/x-ip2" },
                { "isu", "video/x-isvideo" },
                { "it", "audio/it" },
                { "iv", "application/x-inventor" },
                { "ivr", "i-world/i-vrml" },
                { "ivy", "application/x-livescreen" },
                { "jam", "audio/x-jam" },
                { "jav", "text/plain" },
                { "java", "text/plain" },
                { "jcm", "application/x-java-commerce" },
                { "jfif", "image/jpeg" },
                { "jfif-tbnl", "image/jpeg" },
                { "jpe", "image/jpeg" },
                { "jpeg", "image/jpeg" },
                { "jpg", "image/jpeg" },
                { "jps", "image/x-jps" },
                { "js", "application/x-javascript" },
                { "jut", "image/jutvision" },
                { "kar", "audio/midi" },
                { "ksh", "application/x-ksh" },
                { "la", "audio/nspaudio" },
                { "lam", "audio/x-liveaudio" },
                { "latex", "application/x-latex" },
                { "lha", "application/lha" },
                { "lhx", "application/octet-stream" },
                { "list", "text/plain" },
                { "lma", "audio/nspaudio" },
                { "log", "text/plain" },
                { "lsp", "application/x-lisp" },
                { "lst", "text/plain" },
                { "lsx", "text/x-la-asf" },
                { "ltx", "application/x-latex" },
                { "lzh", "application/octet-stream" },
                { "lzx", "application/lzx" },
                { "m", "text/plain" },
                { "m1v", "video/mpeg" },
                { "m2a", "audio/mpeg" },
                { "m2v", "video/mpeg" },
                { "m3u", "audio/x-mpequrl" },
                { "man", "application/x-troff-man" },
                { "map", "application/x-navimap" },
                { "mar", "text/plain" },
                { "mbd", "application/mbedlet" },
                { "mc$", "application/x-magic-cap-package-1.0" },
                { "mcd", "application/mcad" },
                { "mcf", "image/vasa" },
                { "mcp", "application/netmc" },
                { "me", "application/x-troff-me" },
                { "mht", "message/rfc822" },
                { "mhtml", "message/rfc822" },
                { "mid", "audio/midi" },
                { "midi", "audio/midi" },
                { "mif", "application/x-frame" },
                { "mime", "message/rfc822" },
                { "mjf", "audio/x-vnd.audioexplosion.mjuicemediafile" },
                { "mjpg", "video/x-motion-jpeg" },
                { "mm", "application/base64" },
                { "mme", "application/base64" },
                { "mod", "audio/mod" },
                { "moov", "video/quicktime" },
                { "mov", "video/quicktime" },
                { "movie", "video/x-sgi-movie" },
                { "mp2", "audio/mpeg" },
                { "mp3", "audio/mpeg3" },
                { "mpa", "audio/mpeg" },
                { "mpc", "application/x-project" },
                { "mpe", "video/mpeg" },
                { "mpeg", "video/mpeg" },
                { "mpg", "video/mpeg" },
                { "mpga", "audio/mpeg" },
                { "mpp", "application/vnd.ms-project" },
                { "mpt", "application/x-project" },
                { "mpv", "application/x-project" },
                { "mpx", "application/x-project" },
                { "mrc", "application/marc" },
                { "ms", "application/x-troff-ms" },
                { "mv", "video/x-sgi-movie" },
                { "my", "audio/make" },
                { "mzz", "application/x-vnd.audioexplosion.mzz" },
                { "nap", "image/naplps" },
                { "naplps", "image/naplps" },
                { "nc", "application/x-netcdf" },
                { "ncm", "application/vnd.nokia.configuration-message" },
                { "nif", "image/x-niff" },
                { "niff", "image/x-niff" },
                { "nix", "application/x-mix-transfer" },
                { "nsc", "application/x-conference" },
                { "nvd", "application/x-navidoc" },
                { "o", "application/octet-stream" },
                { "oda", "application/oda" },
                { "omc", "application/x-omc" },
                { "omcd", "application/x-omcdatamaker" },
                { "omcr", "application/x-omcregerator" },
                { "p", "text/x-pascal" },
                { "p10", "application/pkcs10" },
                { "p12", "application/pkcs-12" },
                { "p7a", "application/x-pkcs7-signature" },
                { "p7c", "application/pkcs7-mime" },
                { "pas", "text/pascal" },
                { "pbm", "image/x-portable-bitmap" },
                { "pcl", "application/vnd.hp-pcl" },
                { "pct", "image/x-pict" },
                { "pcx", "image/x-pcx" },
                { "pdf", "application/pdf" },
                { "pfunk", "audio/make" },
                { "pgm", "image/x-portable-graymap" },
                { "pic", "image/pict" },
                { "pict", "image/pict" },
                { "pkg", "application/x-newton-compatible-pkg" },
                { "pko", "application/vnd.ms-pki.pko" },
                { "pl", "text/plain" },
                { "plx", "application/x-pixclscript" },
                { "pm", "image/x-xpixmap" },
                { "png", "image/png" },
                { "pnm", "application/x-portable-anymap" },
                { "pot", "application/mspowerpoint" },
                { "pov", "model/x-pov" },
                { "ppa", "application/vnd.ms-powerpoint" },
                { "ppm", "image/x-portable-pixmap" },
                { "pps", "application/mspowerpoint" },
                { "ppt", "application/mspowerpoint" },
                { "ppz", "application/mspowerpoint" },
                { "pre", "application/x-freelance" },
                { "prt", "application/pro_eng" },
                { "ps", "application/postscript" },
                { "psd", "application/octet-stream" },
                { "pvu", "paleovu/x-pv" },
                { "pwz", "application/vnd.ms-powerpoint" },
                { "py", "text/x-script.phyton" },
                { "pyc", "applicaiton/x-bytecode.python" },
                { "qcp", "audio/vnd.qcelp" },
                { "qd3", "x-world/x-3dmf" },
                { "qd3d", "x-world/x-3dmf" },
                { "qif", "image/x-quicktime" },
                { "qt", "video/quicktime" },
                { "qtc", "video/x-qtc" },
                { "qti", "image/x-quicktime" },
                { "qtif", "image/x-quicktime" },
                { "ra", "audio/x-pn-realaudio" },
                { "ram", "audio/x-pn-realaudio" },
                { "ras", "application/x-cmu-raster" },
                { "rast", "image/cmu-raster" },
                { "rexx", "text/x-script.rexx" },
                { "rf", "image/vnd.rn-realflash" },
                { "rgb", "image/x-rgb" },
                { "rm", "application/vnd.rn-realmedia" },
                { "rmi", "audio/mid" },
                { "rmm", "audio/x-pn-realaudio" },
                { "rmp", "audio/x-pn-realaudio" },
                { "rng", "application/ringing-tones" },
                { "rnx", "application/vnd.rn-realplayer" },
                { "roff", "application/x-troff" },
                { "rp", "image/vnd.rn-realpix" },
                { "rpm", "audio/x-pn-realaudio-plugin" },
                { "rt", "text/richtext" },
                { "rtf", "text/richtext" },
                { "rtx", "application/rtf" },
                { "rv", "video/vnd.rn-realvideo" },
                { "s", "text/x-asm" },
                { "s3m", "audio/s3m" },
                { "saveme", "application/octet-stream" },
                { "sbk", "application/x-tbook" },
                { "scm", "application/x-lotusscreencam" },
                { "sdml", "text/plain" },
                { "sdp", "application/sdp" },
                { "sdr", "application/sounder" },
                { "sea", "application/sea" },
                { "set", "application/set" },
                { "sgm", "text/sgml" },
                { "sgml", "text/sgml" },
                { "sh", "application/x-bsh" },
                { "shtml", "text/html" },
                { "sid", "audio/x-psid" },
                { "sit", "application/x-sit" },
                { "skd", "application/x-koan" },
                { "skm", "application/x-koan" },
                { "skp", "application/x-koan" },
                { "skt", "application/x-koan" },
                { "sl", "application/x-seelogo" },
                { "smi", "application/smil" },
                { "smil", "application/smil" },
                { "snd", "audio/basic" },
                { "sol", "application/solids" },
                { "spc", "application/x-pkcs7-certificates" },
                { "spl", "application/futuresplash" },
                { "spr", "application/x-sprite" },
                { "sprite", "application/x-sprite" },
                { "src", "application/x-wais-source" },
                { "ssi", "text/x-server-parsed-html" },
                { "ssm", "application/streamingmedia" },
                { "sst", "application/vnd.ms-pki.certstore" },
                { "step", "application/step" },
                { "stl", "application/sla" },
                { "stp", "application/step" },
                { "sv4cpio", "application/x-sv4cpio" },
                { "sv4crc", "application/x-sv4crc" },
                { "svf", "image/vnd.dwg" },
                { "svr", "application/x-world" },
                { "swf", "application/x-shockwave-flash" },
                { "t", "application/x-troff" },
                { "talk", "text/x-speech" },
                { "tar", "application/x-tar" },
                { "tbk", "application/toolbook" },
                { "tcl", "application/x-tcl" },
                { "tcsh", "text/x-script.tcsh" },
                { "tex", "application/x-tex" },
                { "texi", "application/x-texinfo" },
                { "texinfo", "application/x-texinfo" },
                { "text", "text/plain" },
                { "tgz", "application/x-compressed" },
                { "tif", "image/tiff" },
                { "tr", "application/x-troff" },
                { "tsi", "audio/tsp-audio" },
                { "tsp", "audio/tsplayer" },
                { "tsv", "text/tab-separated-values" },
                { "turbot", "image/florian" },
                { "txt", "text/plain" },
                { "uil", "text/x-uil" },
                { "uni", "text/uri-list" },
                { "unis", "text/uri-list" },
                { "unv", "application/i-deas" },
                { "uri", "text/uri-list" },
                { "uris", "text/uri-list" },
                { "ustar", "application/x-ustar" },
                { "uu", "application/octet-stream" },
                { "vcd", "application/x-cdlink" },
                { "vcs", "text/x-vcalendar" },
                { "vda", "application/vda" },
                { "vdo", "video/vdo" },
                { "vew", "application/groupwise" },
                { "viv", "video/vivo" },
                { "vivo", "video/vivo" },
                { "vmd", "application/vocaltec-media-desc" },
                { "vmf", "application/vocaltec-media-file" },
                { "voc", "audio/voc" },
                { "vos", "video/vosaic" },
                { "vox", "audio/voxware" },
                { "vqe", "audio/x-twinvq-plugin" },
                { "vqf", "audio/x-twinvq" },
                { "vql", "audio/x-twinvq-plugin" },
                { "vrml", "application/x-vrml" },
                { "vrt", "x-world/x-vrt" },
                { "vsd", "application/x-visio" },
                { "vst", "application/x-visio" },
                { "vsw", "application/x-visio" },
                { "w60", "application/wordperfect6.0" },
                { "w61", "application/wordperfect6.1" },
                { "w6w", "application/msword" },
                { "wav", "audio/wav" },
                { "wb1", "application/x-qpro" },
                { "wbmp", "image/vnd.wap.wbmp" },
                { "web", "application/vnd.xara" },
                { "wiz", "application/msword" },
                { "wk1", "application/x-123" },
                { "wmf", "windows/metafile" },
                { "wml", "text/vnd.wap.wml" },
                { "wmlc", "application/vnd.wap.wmlc" },
                { "wmls", "text/vnd.wap.wmlscript" },
                { "wmlsc", "application/vnd.wap.wmlscriptc" },
                { "word", "application/msword" },
                { "wp", "application/wordperfect" },
                { "wp5", "application/wordperfect" },
                { "wp6", "application/wordperfect" },
                { "wpd", "application/wordperfect" },
                { "wq1", "application/x-lotus" },
                { "wri", "application/mswrite" },
                { "wrl", "application/x-world" },
                { "wrz", "model/vrml" },
                { "wsc", "text/scriplet" },
                { "wsrc", "application/x-wais-source" },
                { "wtk", "application/x-wintalk" },
                { "xbm", "image/x-xbitmap" },
                { "xdr", "video/x-amt-demorun" },
                { "xgz", "xgl/drawing" },
                { "xif", "image/vnd.xiff" },
                { "xl", "application/excel" },
                { "xla", "application/excel" },
                { "xlb", "application/excel" },
                { "xlc", "application/excel" },
                { "xld", "application/excel" },
                { "xlk", "application/excel" },
                { "xll", "application/excel" },
                { "xlm", "application/excel" },
                { "xls", "application/excel" },
                { "xlsx", "application/excel" },
                { "xlt", "application/excel" },
                { "xlv", "application/excel" },
                { "xlw", "application/excel" },
                { "xm", "audio/xm" },
                { "xml", "text/xml" },
                { "xmz", "xgl/movie" },
                { "xpix", "application/x-vnd.ls-xpix" },
                { "xpm", "image/x-xpixmap" },
                { "x-png", "image/png" },
                { "xsr", "video/x-amt-showrun" },
                { "xwd", "image/x-xwd" },
                { "xyz", "chemical/x-pdb" },
                { "z", "application/x-compress" },
                { "zip", "application/x-compressed" },
                { "zoo", "application/octet-stream" },
                { "zsh", "text/x-script.zsh" }
            };
        }
    }
}