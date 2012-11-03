::BnfToDfa.exe --all-marks xcap-path.bnf "xcap-path" all-marks.txt
BnfToDfa.exe xcap-path.bnf xcap-path.mrk "xcap-path"
DfaToCSharp.exe xcap-path.xml Server.Xcap XcapUriParser

move Server.Xcap.dfa ..\
move XcapUriParser.cs ..\