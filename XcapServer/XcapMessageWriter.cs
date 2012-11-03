using System;
using Http.Message;

namespace Server.Xcap
{
	static class XcapMessageWriter
	{
		public static void WriteNotFinishedResponse(this HttpMessageWriter writer, ContentType contentType)
		{
			writer.WriteStatusLine(StatusCodes.OK);
			writer.WriteAccessControlHeaders();
			writer.WriteContentType(contentType);
		}

		public static void WriteResponse(this HttpMessageWriter writer, StatusCodes statusCodes, ContentType contentType, byte[] content)
		{
			writer.WriteStatusLine(statusCodes);
			writer.WriteAccessControlHeaders();
			writer.WriteContentType(contentType);
			writer.WriteContentLength(content.Length);
			writer.WriteCRLF();
			writer.Write(content);
		}

		public static void WriteEmptyResponse(this HttpMessageWriter writer, StatusCodes statusCode)
		{
			writer.WriteStatusLine(statusCode);
			writer.WriteAccessControlHeaders();
			writer.WriteContentLength(0);
			writer.WriteCRLF();
		}

		public static void WriteOptionsResponse(this HttpMessageWriter writer)
		{
			writer.WriteStatusLine(StatusCodes.OK);
			writer.WriteAccessControlHeaders();
			writer.WriteAllow(Methods.Get, Methods.Options);
			writer.WriteContentLength(0);
			writer.WriteCRLF();
		}

		public static void WriteAccessControlHeaders(this HttpMessageWriter writer)
		{
			writer.WriteAccessControlAllowOrigin(true);
			writer.WriteAccessControlAllowCredentials(true);
			writer.WriteAccessControlAllowHeaders(System.Text.Encoding.UTF8.GetBytes("Authorization"));
			writer.WriteAccessControlExposeHeaders(System.Text.Encoding.UTF8.GetBytes("WWW-Authenticate"));
			writer.WriteAccessControlAllowMethods(Methods.Get, Methods.Options);
		}
	}
}
