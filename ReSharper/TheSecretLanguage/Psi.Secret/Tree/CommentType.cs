// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   CommentType.cs
// </summary>
// ***********************************************************************

namespace JetBrains.ReSharper.Psi.Secret.Tree
{
    public enum CommentType : byte
    {
        END_OF_LINE_COMMENT, // example: //
        MULTILINE_COMMENT, // example: (* *)
        DOC_COMMENT // example: ///
    }
}
