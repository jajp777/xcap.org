using System;
using System.Text;
using Http.Message;

namespace Server.Xcap
{
	static class XcapMessageWriter
	{
		private static byte[] accessControlAllowHeaders = Encoding.UTF8.GetBytes("Authorization, If-Match, Content-Type");
		private static byte[] accessControlExposeHeaders = Encoding.UTF8.GetBytes("WWW-Authenticate, ETag");

		public static void WriteNotFinishedResponse(this HttpMessageWriter writer, StatusCodes statusCode, ContentType contentType)
		{
			writer.WriteStatusLine(statusCode);
			writer.WriteAuxiliaryHeaders();
			if (contentType != ContentType.None)
				writer.WriteContentType(contentType);
			else
				writer.WriteContentLength(0);
		}

		public static void WriteResponse(this HttpMessageWriter writer, StatusCodes statusCodes, ContentType contentType, byte[] content)
		{
			writer.WriteStatusLine(statusCodes);
			writer.WriteAuxiliaryHeaders();
			writer.WriteContentType(contentType);
			writer.WriteContentLength(content.Length);
			writer.WriteCRLF();
			writer.Write(content);
		}

		public static void WriteEmptyResponse(this HttpMessageWriter writer, StatusCodes statusCode)
		{
			writer.WriteStatusLine(statusCode);
			writer.WriteAuxiliaryHeaders();
			writer.WriteContentLength(0);
			writer.WriteCRLF();
		}

		public static void WriteOptionsResponse(this HttpMessageWriter writer)
		{
			writer.WriteStatusLine(StatusCodes.OK);
			writer.WriteAuxiliaryHeaders();
			writer.WriteAllow(Methods.Get, Methods.Options);
			writer.WriteContentLength(0);
			writer.WriteCRLF();
		}

		public static void WriteAuxiliaryHeaders(this HttpMessageWriter writer)
		{
			writer.WriteCacheControlNoCache();

			writer.WriteAccessControlAllowOrigin(true);
			writer.WriteAccessControlAllowCredentials(true);
			writer.WriteAccessControlAllowHeaders(accessControlAllowHeaders);
			writer.WriteAccessControlExposeHeaders(accessControlExposeHeaders);
			writer.WriteAccessControlAllowMethods(Methods.Get, Methods.Put, Methods.Options);
		}
	}
}
