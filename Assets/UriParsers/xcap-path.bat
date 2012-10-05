::BnfToDfa.exe --all-marks rfc4825.bnf "xcap-path" all-marks.txt
BnfToDfa.exe xcap-path.bnf xcap-path.mrk "xcap-path"
DfaToCSharp.exe xcap-path.xml Xcap.PathParser XcapPathParser