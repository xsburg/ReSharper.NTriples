// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SyntaxHighlightingStage.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Media;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Application.Parts;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl.Markup;
using JetBrains.Util;
using JetBrains.VsIntegration.Application;
using JetBrains.VsIntegration.DevTen.Markup;
using JetBrains.VsIntegration.Install.VsPackageAssembly;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using ReSharper.NTriples.CodeInspections;
using ReSharper.NTriples.CodeInspections.Highlightings;
using ReSharper.NTriples.Parsing;
using ReSharper.NTriples.Tree;

[assembly: RegisterHighlighter(NTriplesHighlightingAtttributes.String, DarkForegroundColor = "LightBlue", EffectType = EffectType.TEXT, ForegroundColor = "DarkBlue", Layer = 2000, VSPriority = 40)]
[assembly: RegisterHighlighter(NTriplesHighlightingAtttributes.DataString, DarkForegroundColor = "LightBlue", EffectType = EffectType.TEXT, ForegroundColor = "DarkBlue", Layer = 2000, VSPriority = 40)]
[assembly: RegisterHighlighter(NTriplesHighlightingAtttributes.Literal, DarkForegroundColor = "LightBlue", EffectType = EffectType.TEXT, ForegroundColor = "DarkBlue", Layer = 2000, VSPriority = 40)]
[assembly: RegisterHighlighter(NTriplesHighlightingAtttributes.Comment, DarkForegroundColor = "LightBlue", EffectType = EffectType.TEXT, ForegroundColor = "DarkBlue", Layer = 2000, VSPriority = 40)]
[assembly: RegisterHighlighter(NTriplesHighlightingAtttributes.Keyword, DarkForegroundColor = "LightBlue", EffectType = EffectType.TEXT, ForegroundColor = "DarkBlue", Layer = 2000, VSPriority = 40)]

[ClassificationType(ClassificationTypeNames = Name)]
[Order(After = VsAnalysisPriorityClassificationDefinition.Name, Before = VsHighlightPriorityClassificationDefinition.Name)]
[Export(typeof(EditorFormatDefinition))]
[Name(Name)]
[DisplayName(Name)]
[UserVisible(true)]
internal class DeadCode2ClassificationDefinition : ClassificationFormatDefinition
{
    private const string Name = NTriplesHighlightingAtttributes.String;
    public DeadCode2ClassificationDefinition()
    {
        DisplayName = Name;
        //ForegroundColor = Colors.LightGray;
        ForegroundOpacity = 0.5;
    }
    [Export, Name(Name), BaseDefinition("formal language")]
    internal ClassificationTypeDefinition ClassificationTypeDefinition;
}

[Order(After = VsAnalysisPriorityClassificationDefinition.Name, Before = VsHighlightPriorityClassificationDefinition.Name)]
[Export(typeof(EditorFormatDefinition))]
[ClassificationType(ClassificationTypeNames = NTriplesHighlightingAtttributes.DataString)]
[Name(NTriplesHighlightingAtttributes.DataString)]
[UserVisible(true)]
internal class DataStringClassificationFormatDefinition : ClassificationFormatDefinition
{
    public DataStringClassificationFormatDefinition()
    {
        this.DisplayName = "HAHA";
        this.ForegroundColor = new Color?(Color.FromRgb(223, 40, 87));
    }
    [Export, Name(NTriplesHighlightingAtttributes.DataString), BaseDefinition("formal language")]
    internal ClassificationTypeDefinition ClassificationTypeDefinition;
}
[Order(After = VsAnalysisPriorityClassificationDefinition.Name, Before = VsHighlightPriorityClassificationDefinition.Name)]
[Export(typeof(EditorFormatDefinition))]
[ClassificationType(ClassificationTypeNames = NTriplesHighlightingAtttributes.Literal)]
[Name(NTriplesHighlightingAtttributes.Literal)]
[UserVisible(true)]
internal class LiteralClassificationFormatDefinition : ClassificationFormatDefinition
{
    public LiteralClassificationFormatDefinition()
    {
        this.DisplayName = "HAHA";
        this.ForegroundColor = new Color?(Color.FromRgb(223, 40, 87));
    }
    [Export, Name(NTriplesHighlightingAtttributes.Literal), BaseDefinition("formal language")]
    internal ClassificationTypeDefinition ClassificationTypeDefinition;
}
[Order(After = VsAnalysisPriorityClassificationDefinition.Name, Before = VsHighlightPriorityClassificationDefinition.Name)]
[Export(typeof(EditorFormatDefinition))]
[ClassificationType(ClassificationTypeNames = NTriplesHighlightingAtttributes.Comment)]
[Name(NTriplesHighlightingAtttributes.Comment)]
[UserVisible(true)]
internal class CommentClassificationFormatDefinition : ClassificationFormatDefinition
{
    public CommentClassificationFormatDefinition()
    {
        this.DisplayName = "HAHA";
        this.ForegroundColor = new Color?(Color.FromRgb(223, 40, 87));
    }
    [Export, Name(NTriplesHighlightingAtttributes.Comment), BaseDefinition("formal language")]
    internal ClassificationTypeDefinition ClassificationTypeDefinition;
}
[Order(After = VsAnalysisPriorityClassificationDefinition.Name, Before = VsHighlightPriorityClassificationDefinition.Name)]
[Export(typeof(EditorFormatDefinition))]
[ClassificationType(ClassificationTypeNames = NTriplesHighlightingAtttributes.Keyword)]
[Name(NTriplesHighlightingAtttributes.Keyword)]
[UserVisible(true)]
internal class KeywordClassificationFormatDefinition : ClassificationFormatDefinition
{
    public KeywordClassificationFormatDefinition()
    {
        this.DisplayName = "HAHA";
        this.ForegroundColor = new Color?(Color.FromRgb(223, 40, 87));
    }
    [Export, Name(NTriplesHighlightingAtttributes.Keyword), BaseDefinition("formal language")]
    internal ClassificationTypeDefinition ClassificationTypeDefinition;
}

namespace ReSharper.NTriples.CodeInspections
{
    public static class NTriplesHighlightingAtttributes
    {
        public const string String = "ReSharper NTriples String";
        public const string DataString = "ReSharper NTriples Data String";
        public const string Literal = "ReSharper NTriples Literal";
        public const string Comment = "ReSharper NTriples Comment";
        public const string Keyword = "ReSharper NTriples Keyword";
    }

    [DaemonStage]
    public class SyntaxHighlightingStage : NTriplesDaemonStageBase
    {
        public override IEnumerable<IDaemonStageProcess> CreateProcess(
            IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
        {
            if (!this.IsSupported(process.SourceFile))
            {
                return EmptyList<IDaemonStageProcess>.InstanceList;
            }

            return new[]
                {
                    new SyntaxHighlightingProcess(process, settings)
                };
        }

        public override ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settings)
        {
            return ErrorStripeRequest.STRIPE_AND_ERRORS;
        }

        private class SyntaxHighlightingProcess : NTriplesIncrementalDaemonStageProcessBase
        {
            public SyntaxHighlightingProcess(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
                : base(process, settingsStore)
            {
            }

            public override void VisitNode(ITreeNode node, IHighlightingConsumer consumer)
            {
                // Tree level highlighting
                //
                const string prefixHighlighting = HighlightingAttributeIds.NAMESPACE_IDENTIFIER_ATTRIBUTE;
                if (node is IPrefix || node is IPrefixName)
                {
                    this.AddSyntaxHighlighting(consumer, node, prefixHighlighting);
                }
                else if (node is ILocalName)
                {
                    this.AddSyntaxHighlighting(consumer, node, HighlightingAttributeIds.METHOD_IDENTIFIER_ATTRIBUTE);
                }
                else if (node is IVariableIdentifier)
                {
                    this.AddSyntaxHighlighting(consumer, node, HighlightingAttributeIds.FIELD_IDENTIFIER_ATTRIBUTE);
                }
                else if (node is ITokenNode)
                {
                    // Token level highlighting
                    // 
                    var token = node as ITokenNode;
                    if (token.GetTokenType().IsStringLiteral)
                    {
                        if (token.Parent is IDataLiteral && token.Parent.LastChild != token)
                        {
                            this.AddSyntaxHighlighting(consumer, node, "String");
                        }
                        else
                        {
                            this.AddSyntaxHighlighting(consumer, node, "String");
                        }
                    }
                    else if (token.GetTokenType().IsComment)
                    {
                        this.AddSyntaxHighlighting(consumer, node, "Comment");
                    }
                    else if (token.GetTokenType().IsKeyword)
                    {
                        this.AddSyntaxHighlighting(consumer, node, "Keyword");
                    }
                    else if (token.GetTokenType().IsConstantLiteral)
                    {
                        this.AddSyntaxHighlighting(consumer, node, "Literal");
                    }
                    else if (token.GetTokenType() == SecretTokenType.NAMESPACE_SEPARATOR)
                    {
                        this.AddSyntaxHighlighting(consumer, node, prefixHighlighting);
                    }
                }
            }

            private void AddHighlighting([NotNull] IHighlightingConsumer consumer, IHighlighting highlighting)
            {
                consumer.AddHighlighting(highlighting, this.File);
            }

            private void AddSyntaxHighlighting([NotNull] IHighlightingConsumer consumer, ITreeNode node, string attributeId)
            {
                consumer.AddHighlighting(new NTriplesSyntaxHighlighting(node, attributeId), this.File);
            }
        }
    }
}
