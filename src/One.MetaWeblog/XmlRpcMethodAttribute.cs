﻿using System;

namespace One.MetaWeblog
{
  public class XmlRpcMethodAttribute : Attribute
  {

    public XmlRpcMethodAttribute(string methodName)
    {
      MethodName = methodName;
    }

    public string MethodName { get; set; }
  }
}