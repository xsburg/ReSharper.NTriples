//------------------------------------------------------------------------------
// <auto-generated>
//     Generated by IntelliJ parserGen
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#pragma warning disable 0168, 0219, 0108, 0414
// ReSharper disable RedundantNameQualifier
namespace JetBrains.ReSharper.Psi.Secret.Parsing {
  public sealed class ErrorMessages {
    private ErrorMessages () {}
    private static string ourMessage_0 = null;
    public static string GetErrorMessage0 ()
    {
      if (ourMessage_0 == null)
      {
        ourMessage_0 = JetBrains.ReSharper.Psi.Secret.Parsing.ParserMessages.GetUnexpectedTokenMessage();
      }
      return ourMessage_0;
    }
  
    private static string ourMessage_1 = null;
    public static string GetErrorMessage1 ()
    {
      if (ourMessage_1 == null)
      {
        ourMessage_1 = JetBrains.ReSharper.Psi.Secret.Parsing.ParserMessages.GetExpectedMessage("\'" + JetBrains.ReSharper.Psi.Secret.Tree.Impl.TokenType.IDENTIFIER.TokenRepresentation + "\'","\'" + JetBrains.ReSharper.Psi.Secret.Tree.Impl.TokenType.URI_BEGIN.TokenRepresentation + "\'");
      }
      return ourMessage_1;
    }
  
    private static string ourMessage_2 = null;
    public static string GetErrorMessage2 ()
    {
      if (ourMessage_2 == null)
      {
        ourMessage_2 = JetBrains.ReSharper.Psi.Secret.Parsing.ParserMessages.GetExpectedMessage(JetBrains.ReSharper.Psi.Secret.Parsing.ParserMessages.GetString(JetBrains.ReSharper.Psi.Secret.Parsing.ParserMessages.IDS_IDENTIFIER));
      }
      return ourMessage_2;
    }
  
    private static string ourMessage_3 = null;
    public static string GetErrorMessage3 ()
    {
      if (ourMessage_3 == null)
      {
        ourMessage_3 = JetBrains.ReSharper.Psi.Secret.Parsing.ParserMessages.GetExpectedMessage(JetBrains.ReSharper.Psi.Secret.Parsing.ParserMessages.GetString(JetBrains.ReSharper.Psi.Secret.Parsing.ParserMessages.IDS_LITERAL));
      }
      return ourMessage_3;
    }
  
    private static string ourMessage_4 = null;
    public static string GetErrorMessage4 ()
    {
      if (ourMessage_4 == null)
      {
        ourMessage_4 = JetBrains.ReSharper.Psi.Secret.Parsing.ParserMessages.GetExpectedMessage(JetBrains.ReSharper.Psi.Secret.Parsing.ParserMessages.GetString(JetBrains.ReSharper.Psi.Secret.Parsing.ParserMessages.IDS_LITERAL_KEYWORDS));
      }
      return ourMessage_4;
    }
  
  }
}