#if (!OPTIMIZED && !OPTIMIZED2)
using System;
using System.Text;
using System.IO;
using System.IO.Compression;
using Base.Message;

namespace Xcap.PathParser
{
public enum Usages
{
None,
Users,
Global,
}
public enum Auids
{
None,
RlsResources,
ResourceLists,
}
public enum DocumentNameIds
{
None,
Index,
}
public partial class XcapPathParser
{
public bool Final;
public bool IsFinal { get { return Final; }}
public bool Error;
public bool IsError { get { return Error; }}
private int state;
private int boolExPosition;
public ByteArrayPart Username;
public ByteArrayPart DocumentName;
public Usages Usage;
public Auids Auid;
public DocumentNameIds DocumentNameId;
partial void OnSetDefaultValue();
public void SetDefaultValue()
{
Final = false;
Error = false;
state = State0;
boolExPosition = int.MinValue;
Usage = Usages.None;
Auid = Auids.None;
DocumentNameId = DocumentNameIds.None;
Username.SetDefaultValue();
DocumentName.SetDefaultValue();
OnSetDefaultValue();
}
public void SetArray(byte[] bytes)
{
Username.Bytes = bytes;
DocumentName.Bytes = bytes;
}
#region enum States
const int State0 = 0;
const int State1 = 1;
const int State2 = 2;
const int State3 = 3;
const int State4 = 4;
const int State5 = 5;
const int State6 = 6;
const int State7 = 7;
const int State8 = 8;
const int State9 = 9;
const int State10 = 10;
const int State11 = 11;
const int State12 = 12;
const int State13 = 13;
const int State14 = 14;
const int State15 = 15;
const int State16 = 16;
const int State17 = 17;
const int State18 = 18;
const int State19 = 19;
const int State20 = 20;
const int State21 = 21;
const int State22 = 22;
const int State23 = 23;
const int State24 = 24;
const int State25 = 25;
const int State26 = 26;
const int State27 = 27;
const int State28 = 28;
const int State29 = 29;
const int State30 = 30;
const int State31 = 31;
const int State32 = 32;
const int State33 = 33;
const int State34 = 34;
const int State35 = 35;
const int State36 = 36;
const int State37 = 37;
const int State38 = 38;
const int State39 = 39;
const int State40 = 40;
const int State41 = 41;
const int State42 = 42;
const int State43 = 43;
const int State44 = 44;
const int State45 = 45;
const int State46 = 46;
const int State47 = 47;
const int State48 = 48;
const int State49 = 49;
const int State50 = 50;
const int State51 = 51;
const int State52 = 52;
const int State53 = 53;
const int State54 = 54;
const int State55 = 55;
const int State56 = 56;
#endregion
#region States Tables
private static int[] table0;
private static int[] table1;
private static int[] table2;
private static int[] table3;
private static int[] table4;
private static int[] table5;
private static int[] table6;
private static int[] table7;
private static int[] table8;
private static int[] table9;
private static int[] table10;
private static int[] table11;
private static int[] table12;
private static int[] table13;
private static int[] table14;
private static int[] table15;
private static int[] table16;
private static int[] table17;
private static int[] table18;
private static int[] table19;
private static int[] table20;
private static int[] table21;
private static int[] table22;
private static int[] table23;
private static int[] table24;
private static int[] table25;
private static int[] table26;
private static int[] table27;
private static int[] table28;
private static int[] table29;
private static int[] table30;
private static int[] table31;
private static int[] table32;
private static int[] table33;
private static int[] table34;
private static int[] table35;
private static int[] table36;
private static int[] table37;
private static int[] table38;
private static int[] table39;
private static int[] table40;
private static int[] table41;
private static int[] table42;
private static int[] table43;
private static int[] table44;
private static int[] table45;
private static int[] table46;
private static int[] table47;
private static int[] table48;
private static int[] table49;
private static int[] table50;
private static int[] table51;
private static int[] table52;
private static int[] table53;
private static int[] table54;
private static int[] table55;
#endregion
#region void LoadTables(..)
public static void LoadTables()
{
LoadTables("");
}
public static void LoadTables(string path)
{
const int maxItems = byte.MaxValue + 1;
const int maxBytes = sizeof(Int32) * maxItems;
using (var reader = new DeflateStream(File.OpenRead(path+"\\Xcap.PathParser.dfa"), CompressionMode.Decompress))
{
byte[] buffer = new byte[maxBytes];
table0 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table0, 0, maxBytes);
table1 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table1, 0, maxBytes);
table2 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table2, 0, maxBytes);
table3 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table3, 0, maxBytes);
table4 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table4, 0, maxBytes);
table5 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table5, 0, maxBytes);
table6 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table6, 0, maxBytes);
table7 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table7, 0, maxBytes);
table8 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table8, 0, maxBytes);
table9 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table9, 0, maxBytes);
table10 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table10, 0, maxBytes);
table11 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table11, 0, maxBytes);
table12 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table12, 0, maxBytes);
table13 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table13, 0, maxBytes);
table14 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table14, 0, maxBytes);
table15 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table15, 0, maxBytes);
table16 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table16, 0, maxBytes);
table17 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table17, 0, maxBytes);
table18 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table18, 0, maxBytes);
table19 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table19, 0, maxBytes);
table20 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table20, 0, maxBytes);
table21 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table21, 0, maxBytes);
table22 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table22, 0, maxBytes);
table23 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table23, 0, maxBytes);
table24 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table24, 0, maxBytes);
table25 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table25, 0, maxBytes);
table26 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table26, 0, maxBytes);
table27 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table27, 0, maxBytes);
table28 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table28, 0, maxBytes);
table29 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table29, 0, maxBytes);
table30 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table30, 0, maxBytes);
table31 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table31, 0, maxBytes);
table32 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table32, 0, maxBytes);
table33 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table33, 0, maxBytes);
table34 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table34, 0, maxBytes);
table35 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table35, 0, maxBytes);
table36 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table36, 0, maxBytes);
table37 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table37, 0, maxBytes);
table38 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table38, 0, maxBytes);
table39 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table39, 0, maxBytes);
table40 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table40, 0, maxBytes);
table41 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table41, 0, maxBytes);
table42 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table42, 0, maxBytes);
table43 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table43, 0, maxBytes);
table44 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table44, 0, maxBytes);
table45 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table45, 0, maxBytes);
table46 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table46, 0, maxBytes);
table47 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table47, 0, maxBytes);
table48 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table48, 0, maxBytes);
table49 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table49, 0, maxBytes);
table50 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table50, 0, maxBytes);
table51 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table51, 0, maxBytes);
table52 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table52, 0, maxBytes);
table53 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table53, 0, maxBytes);
table54 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table54, 0, maxBytes);
table55 = new int[maxItems];
reader.Read(buffer, 0, buffer.Length);
Buffer.BlockCopy(buffer, 0, table55, 0, maxBytes);
}
}
#endregion
partial void OnBeforeParse();
partial void OnAfterParse();
#region int Parse(..)
public bool ParseAll(ArraySegment<byte> data)
{
return ParseAll(data.Array, data.Offset, data.Count);
}
public bool ParseAll(byte[] bytes, int offset, int length)
{
int parsed = 0;
do
{
Final = false;
parsed += Parse(bytes, offset + parsed, length - parsed);
} while (parsed < length && IsFinal);
return IsFinal;
}
public int Parse(ArraySegment<byte> data)
{
return Parse(data.Array, data.Offset, data.Count);
}
public int Parse(byte[] bytes, int offset, int length)
{
OnBeforeParse();
int i = offset;
switch(state)
{
case State0:
state = table0[bytes[i]];
break;
case State1:
state = table1[bytes[i]];
break;
case State2:
state = table2[bytes[i]];
break;
case State3:
state = table3[bytes[i]];
break;
case State4:
state = table4[bytes[i]];
break;
case State5:
state = table5[bytes[i]];
break;
case State6:
state = table6[bytes[i]];
break;
case State7:
state = table7[bytes[i]];
break;
case State8:
state = table8[bytes[i]];
break;
case State9:
state = table9[bytes[i]];
break;
case State10:
state = table10[bytes[i]];
break;
case State11:
state = table11[bytes[i]];
break;
case State12:
state = table12[bytes[i]];
break;
case State13:
state = table13[bytes[i]];
break;
case State14:
state = table14[bytes[i]];
break;
case State15:
state = table15[bytes[i]];
break;
case State16:
state = table16[bytes[i]];
break;
case State17:
state = table17[bytes[i]];
break;
case State18:
state = table18[bytes[i]];
break;
case State19:
state = table19[bytes[i]];
break;
case State20:
state = table20[bytes[i]];
break;
case State21:
state = table21[bytes[i]];
break;
case State22:
state = table22[bytes[i]];
break;
case State23:
state = table23[bytes[i]];
break;
case State24:
state = table24[bytes[i]];
break;
case State25:
state = table25[bytes[i]];
break;
case State26:
state = table26[bytes[i]];
break;
case State27:
state = table27[bytes[i]];
break;
case State28:
state = table28[bytes[i]];
break;
case State29:
state = table29[bytes[i]];
break;
case State30:
state = table30[bytes[i]];
break;
case State31:
state = table31[bytes[i]];
break;
case State32:
state = table32[bytes[i]];
break;
case State33:
state = table33[bytes[i]];
break;
case State34:
state = table34[bytes[i]];
break;
case State35:
state = table35[bytes[i]];
break;
case State36:
state = table36[bytes[i]];
break;
case State37:
state = table37[bytes[i]];
break;
case State38:
state = table38[bytes[i]];
break;
case State39:
state = table39[bytes[i]];
break;
case State40:
state = table40[bytes[i]];
break;
case State41:
state = table41[bytes[i]];
break;
case State42:
state = table42[bytes[i]];
break;
case State43:
state = table43[bytes[i]];
break;
case State44:
state = table44[bytes[i]];
break;
case State45:
state = table45[bytes[i]];
break;
case State46:
state = table46[bytes[i]];
break;
case State47:
state = table47[bytes[i]];
break;
case State48:
state = table48[bytes[i]];
break;
case State49:
state = table49[bytes[i]];
break;
case State50:
state = table50[bytes[i]];
break;
case State51:
state = table51[bytes[i]];
break;
case State52:
state = table52[bytes[i]];
break;
case State53:
state = table53[bytes[i]];
break;
case State54:
state = table54[bytes[i]];
break;
case State55:
state = table55[bytes[i]];
break;
case State56:
Error = true;
goto exit1;
}
i++;
int end = offset + length;
for( ; i < end; i++)
{
switch(state)
{
case State0:
state = table0[bytes[i]];
break;
case State1:
state = table1[bytes[i]];
break;
case State2:
state = table2[bytes[i]];
break;
case State3:
state = table3[bytes[i]];
break;
case State4:
state = table4[bytes[i]];
break;
case State5:
state = table5[bytes[i]];
break;
case State6:
state = table6[bytes[i]];
break;
case State7:
state = table7[bytes[i]];
break;
case State8:
state = table8[bytes[i]];
break;
case State9:
state = table9[bytes[i]];
break;
case State10:
state = table10[bytes[i]];
break;
case State11:
state = table11[bytes[i]];
break;
case State12:
state = table12[bytes[i]];
break;
case State13:
state = table13[bytes[i]];
break;
case State14:
state = table14[bytes[i]];
break;
case State15:
state = table15[bytes[i]];
break;
case State16:
state = table16[bytes[i]];
break;
case State17:
state = table17[bytes[i]];
break;
case State18:
state = table18[bytes[i]];
break;
case State19:
state = table19[bytes[i]];
break;
case State20:
state = table20[bytes[i]];
break;
case State21:
state = table21[bytes[i]];
break;
case State22:
state = table22[bytes[i]];
break;
case State23:
state = table23[bytes[i]];
break;
case State24:
state = table24[bytes[i]];
break;
case State25:
state = table25[bytes[i]];
break;
case State26:
Usage = Usages.Users;
state = table26[bytes[i]];
break;
case State27:
state = table27[bytes[i]];
break;
case State28:
state = table28[bytes[i]];
break;
case State29:
Final = true;
Usage = Usages.Global;
goto exit1;
case State30:
if(Username.Begin < 0)Username.Begin = i;
state = table30[bytes[i]];
break;
case State31:
state = table31[bytes[i]];
break;
case State32:
state = table32[bytes[i]];
break;
case State33:
if(DocumentName.Begin < 0)DocumentName.Begin = i;
state = table33[bytes[i]];
break;
case State34:
Username.End = i;
Final = true;
goto exit1;
case State35:
state = table35[bytes[i]];
break;
case State36:
state = table36[bytes[i]];
break;
case State37:
state = table37[bytes[i]];
break;
case State38:
DocumentName.End = i;
Final = true;
goto exit1;
case State39:
state = table39[bytes[i]];
break;
case State40:
DocumentName.End = i;
Final = true;
goto exit1;
case State41:
state = table41[bytes[i]];
break;
case State42:
state = table42[bytes[i]];
break;
case State43:
state = table43[bytes[i]];
break;
case State44:
state = table44[bytes[i]];
break;
case State45:
DocumentName.End = i;
Final = true;
goto exit1;
case State46:
state = table46[bytes[i]];
break;
case State47:
state = table47[bytes[i]];
break;
case State48:
DocumentName.End = i;
Final = true;
goto exit1;
case State49:
state = table49[bytes[i]];
break;
case State50:
state = table50[bytes[i]];
break;
case State51:
DocumentName.End = i;
Final = true;
goto exit1;
case State52:
state = table52[bytes[i]];
break;
case State53:
Auid = Auids.RlsResources;
state = table53[bytes[i]];
break;
case State54:
DocumentName.End = i;
Final = true;
DocumentNameId = DocumentNameIds.Index;
goto exit1;
case State55:
Auid = Auids.ResourceLists;
state = table55[bytes[i]];
break;
case State56:
i--;
Error = true;
goto exit1;
}
}
switch(state)
{
case State0:
break;
case State1:
break;
case State2:
break;
case State3:
break;
case State4:
break;
case State5:
break;
case State6:
break;
case State7:
break;
case State8:
break;
case State9:
break;
case State10:
break;
case State11:
break;
case State12:
break;
case State13:
break;
case State14:
break;
case State15:
break;
case State16:
break;
case State17:
break;
case State18:
break;
case State19:
break;
case State20:
break;
case State21:
break;
case State22:
break;
case State23:
break;
case State24:
break;
case State25:
break;
case State26:
Usage = Usages.Users;
break;
case State27:
break;
case State28:
break;
case State29:
Final = true;
Usage = Usages.Global;
goto exit1;
case State30:
if(Username.Begin < 0)Username.Begin = i;
break;
case State31:
break;
case State32:
break;
case State33:
if(DocumentName.Begin < 0)DocumentName.Begin = i;
break;
case State34:
Username.End = i;
Final = true;
goto exit1;
case State35:
break;
case State36:
break;
case State37:
break;
case State38:
DocumentName.End = i;
Final = true;
goto exit1;
case State39:
break;
case State40:
DocumentName.End = i;
Final = true;
goto exit1;
case State41:
break;
case State42:
break;
case State43:
break;
case State44:
break;
case State45:
DocumentName.End = i;
Final = true;
goto exit1;
case State46:
break;
case State47:
break;
case State48:
DocumentName.End = i;
Final = true;
goto exit1;
case State49:
break;
case State50:
break;
case State51:
DocumentName.End = i;
Final = true;
goto exit1;
case State52:
break;
case State53:
Auid = Auids.RlsResources;
break;
case State54:
DocumentName.End = i;
Final = true;
DocumentNameId = DocumentNameIds.Index;
goto exit1;
case State55:
Auid = Auids.ResourceLists;
break;
case State56:
i--;
Error = true;
goto exit1;
}
exit1: ;
OnAfterParse();
return i - offset;
}
#endregion
public static readonly byte[] AsciiCodeToHex = new byte[256] {
0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
0xff, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
0xff, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
};
}
}
#endif
