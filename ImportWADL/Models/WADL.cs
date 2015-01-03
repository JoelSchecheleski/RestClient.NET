using RazorEngine;
using SkaCahToa.Rest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ImportWADL.Models
{
	public class WADL
	{
		public class Param
		{
			public string Name { get; set; }
			public string Type { get; set; }
			public string Style { get; set; }
			public bool Required { get; set; }
		}

		public class Request
		{
			public List<Param> Params { get; set; }
		}

		public class Representation
		{
			public string MediaType { get; set; }
			public string Element { get; set; }
		}

		public class Response
		{
			public int Status { get; set; }
			public Representation representation { get; set; }
		}

		public class Method
		{
			public string Name { get; set; }
			public string Id { get; set; }
			public Request request { get; set; }
			public List<Response> response { get; set; }
		}

		public class Resource
		{
			public string Path { get; set; }
			public Method method { get; set; }
		}

		public class Include
		{
			public string Href { get; set; }
		}

		public List<Include> Grammars { get; set; }
		public List<Resource> Resources { get; set; }

		public string BaseURL { get; set; }

		public WADL(string filename)
		{
			Grammars = new List<Include>();
			Resources = new List<Resource>();

			using (XmlReader reader = XmlReader.Create(filename))
				ReadFromXml(reader);
		}

		protected void ReadFromXml(XmlReader reader)
		{
			bool inGrammar = false;
			Resource resource = null;
			Response response = null;

			while (reader.Read())
			{
				switch (reader.Name)
				{
					case "grammars":
						inGrammar = reader.IsStartElement();
						break;

					case "include":
						if (inGrammar && reader.IsStartElement())
							Grammars.Add(new Include() { Href = reader.GetAttribute("href") });
						break;

					case "resources":
						if (reader.IsStartElement())
						{
							resource = new Resource();
							BaseURL = reader.GetAttribute("base");
						}
						else
						{
							Resources.Add(resource);
							resource = null;
						}
						break;

					case "resource":
						if (reader.IsStartElement())
							resource.Path = reader.GetAttribute("path");
						break;

					case "method":
						if (reader.IsStartElement())
						{
							resource.method = new Method();
							resource.method.Name = reader.GetAttribute("name");
							resource.method.Id = reader.GetAttribute("id");
						}
						break;

					case "request":
						if (reader.IsStartElement())
						{
							resource.method.request = new Request();
						}
						break;

					case "param":
						if (reader.IsStartElement())
						{
							if (resource.method.request.Params == null)
								resource.method.request.Params = new List<Param>();

							resource.method.request.Params.Add(new Param()
							{
								Name = reader.GetAttribute("name"),
								Style = reader.GetAttribute("style"),
								Type = reader.GetAttribute("type"),
								Required = reader.GetAttribute("required") == "true"
							});
						}
						break;

					case "response":
						if (reader.IsStartElement())
						{
							response = new Response();
							response.Status = Convert.ToInt32(reader.GetAttribute("status"));
						}
						else
						{
							if (resource.method.response == null)
								resource.method.response = new List<Response>();

							resource.method.response.Add(response);
						}
						break;

					case "representation":
						if (reader.IsStartElement())
						{
							response.representation = new Representation()
							{
								MediaType = reader.GetAttribute("mediaType"),
								Element = reader.GetAttribute("element")
							};
						}
						break;
				}
			}
		}

		public string CreateRestClientDotNetClass(RestClientBase.DataTypes dataType, string className, string nameSpace)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = "ImportWADL.Models.RestClientBaseTemplate.cshtml";

			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					string template = reader.ReadToEnd();
					return Razor.Parse(template, new
					{
						DataType = dataType,
						ClassName = className,
						NameSpace = nameSpace,
						WADL = this
					});
				}
			}
		}
	}
}