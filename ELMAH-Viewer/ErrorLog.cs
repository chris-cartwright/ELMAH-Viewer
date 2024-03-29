﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using ELMAH_Viewer.Common;

namespace ELMAH_Viewer
{
	public class ErrorLog : IErrorLog
	{
		private readonly XmlDocument _document;

		private string _allXml;

		public Guid ErrorId { get; set; }
		public string Application { get; set; }
		public string Host { get; set; }
		public string Type { get; set; }
		public string Source { get; set; }
		public string User { get; set; }
		public int StatusCode { get; set; }
		public DateTime TimeUtc { get; set; }
		public string Message { get; set; }
		public int Sequence { get; set; }

		public string AllXml
		{
			get { return _allXml; }
			set
			{
				_allXml = value;

				Stream xsd = Assembly.GetExecutingAssembly().GetManifestResourceStream("ELMAH_Viewer.Misc.ErrorDetails.xsd");
				Debug.Assert(xsd != null, "XSD not properly embedded.");

				List<XmlSchemaException> errors = new List<XmlSchemaException>();
				XmlReaderSettings settings = new XmlReaderSettings();
				settings.Schemas.Add(null, XmlReader.Create(xsd));
				settings.ValidationType = ValidationType.Schema;
				settings.ValidationEventHandler += (sender, args) => errors.Add(args.Exception);

				_document.Load(XmlReader.Create(new StringReader(value), settings));
				if (errors.Count > 0)
				{
					throw new InvalidValueException() { Data = { { "Errors", errors } } };
				}
			}
		}

		public IEnumerable<KeyValuePair<string, string>> FormValues
		{
			get { return GetGrid("//error/form/item"); }
		}

		public IEnumerable<KeyValuePair<string, string>> CookieValues
		{
			get { return GetGrid("//error/cookies/item"); }
		}

		public IEnumerable<KeyValuePair<string, string>> QueryStringValues
		{
			get { return GetGrid("//error/queryString/item"); }
		}

		public IEnumerable<KeyValuePair<string, string>> ServerValues
		{
			get { return GetGrid(@"//error/serverVariables/item[not(starts-with(@name, ""EXCEPTION_""))]"); }
		}

		public IEnumerable<KeyValuePair<string, string>> ExceptionValues
		{
			get { return MaybeJson(GetGrid(@"//error/serverVariables/item[starts-with(@name, ""EXCEPTION_"")]")); }
		}

		public string StackTrace
		{
			get
			{
				XmlNode ret = _document.SelectSingleNode("//error/@detail");
				return ret != null ? ret.InnerText : String.Empty;
			}
		}

		// This should return an array because the ListContainsElements converter doesn't support IEnumerable
		// ReSharper disable once ReturnTypeCanBeEnumerable.Local
		private KeyValuePair<string, string>[] GetGrid(string xpath)
		{
			XmlNodeList nodes = _document.SelectNodes(xpath);
			if (nodes == null)
			{
				return new Dictionary<string, string>().ToArray();
			}

			// ReSharper disable PossibleNullReferenceException
			return (
				from node in nodes.Cast<XmlNode>()
				let r =
					new KeyValuePair<string, string>(
						node.Attributes["name"].Value,
						node.SelectSingleNode("value").Attributes["string"].Value
					)
				orderby r.Key
				select r
			).ToArray();
			// ReSharper restore PossibleNullReferenceException
		}

		private KeyValuePair<string, string>[] MaybeJson(IEnumerable<KeyValuePair<string, string>> data)
		{
			return data.Select(row => new KeyValuePair<string, string>(row.Key, row.Value.MaybeJson())).ToArray();
		}

		public ErrorLog()
		{
			_document = new XmlDocument();
		}

		public ErrorLog(IErrorLog log)
			: this()
		{
			ErrorId = log.ErrorId;
			Application = log.Application;
			Host = log.Host;
			Type = log.Type;
			Source = log.Source;
			User = log.User;
			StatusCode = log.StatusCode;
			TimeUtc = log.TimeUtc;
			Message = log.Message;
			Sequence = log.Sequence;
			AllXml = log.AllXml;
		}
	}
}
