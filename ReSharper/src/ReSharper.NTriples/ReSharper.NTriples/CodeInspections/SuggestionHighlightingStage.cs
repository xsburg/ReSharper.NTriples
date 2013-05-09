using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.UsageChecking;
using JetBrains.Util;

namespace ReSharper.NTriples.CodeInspections
{
    [DaemonStage(StagesBefore = new[] { typeof(WarningHighlightingStage) }, StagesAfter = new[] { typeof(CollectUsagesStage) })]
    public class SuggestionHighlightingStage : NTriplesDaemonStageBase
    {
        public override IEnumerable<IDaemonStageProcess> CreateProcess(
            IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
        {
            if (!this.IsSupported(process.SourceFile))
            {
                return EmptyList<IDaemonStageProcess>.InstanceList;
            }

            return new List<IDaemonStageProcess>
                {
                    new SuggestionHighlighterProcess(process, settings)
                };
        }
    }
}