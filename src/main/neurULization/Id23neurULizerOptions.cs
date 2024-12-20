﻿namespace ei8.Cortex.Coding.d23.neurULization
{
    public interface Id23neurULizerOptions : IneurULizerOptions
    {
        Coding.d23.neurULization.Processors.Writers.IInstanceWriter InstanceWriter { get; }

        Coding.d23.neurULization.Processors.Readers.Inductive.IInstanceReader InductiveInstanceReader { get; }

        IExternalReferenceSet ExternalReferences { get; }
    }
}
