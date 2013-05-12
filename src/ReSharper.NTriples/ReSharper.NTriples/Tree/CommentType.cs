// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   CommentType.cs
// </summary>
// ***********************************************************************

namespace ReSharper.NTriples.Tree
{
    public enum CommentType : byte
    {
        END_OF_LINE_COMMENT, // example: //
        MULTILINE_COMMENT, // example: (* *)
        DOC_COMMENT // example: ///
    }
}
