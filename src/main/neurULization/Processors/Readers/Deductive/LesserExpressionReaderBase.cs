using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using neurUL.Common.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public abstract class LesserExpressionReaderBase
    <
        TGreatGranny, 
        TGreatGrannyParameterSet, 
        TGreatGrannyReader, 
        TGranny,
        TParameterSet,
        TGrannyDerived
    > : 
        ExpressionProcessorBase<
            TGreatGranny,
            TGreatGrannyParameterSet, 
            TGreatGrannyReader,
            TGranny,
            TParameterSet,
            IExpressionParameterSet,
            IExpressionReader,
            IUnitParameterSet
        >,
        ILesserGrannyReader<TGranny, TParameterSet>
        where TGreatGranny : IGranny
        where TGreatGrannyParameterSet : IDeductiveParameterSet
        where TGreatGrannyReader : IGrannyReader<TGreatGranny, TGreatGrannyParameterSet>
        where TGranny : IExpressionGranny, ILesserGranny<TGreatGranny>
        where TParameterSet : IDeductiveParameterSet
        where TGrannyDerived : TGranny, new()
    {
        private readonly IExternalReferenceSet externalReferences;

        protected LesserExpressionReaderBase(
            TGreatGrannyReader greatGrannyReader,
            IExpressionReader expressionReader,
            IExternalReferenceSet externalReferences
        ) : base (greatGrannyReader, expressionReader)
        {
            AssertionConcern.AssertArgumentNotNull(externalReferences, nameof(externalReferences));

            this.externalReferences = externalReferences;
        }

        public IEnumerable<IGrannyQuery> GetQueries(Network network, TParameterSet parameters) =>
            new IGrannyQuery[] {
                new GreatGrannyQuery<TGreatGranny, TGreatGrannyReader, TGreatGrannyParameterSet>(
                    this.greatGrannyProcessor,
                    (n) => this.CreateGreatGrannyParameterSet(parameters)
                ),
                new GreatGrannyQuery<IExpression, IExpressionReader, IExpressionParameterSet>(
                    this.expressionProcessor,
                    (n) => this.CreateExpressionParameterSet(this.externalReferences, parameters, n.Last().Neuron, network)
                )
            };

        public bool TryParse(Network network, TParameterSet parameters, out TGranny result) =>
            this.TryParseAggregate(
                () => new TGrannyDerived(),
                parameters,
                new IGreatGrannyProcess<TGranny>[]
                {
                    new GreatGrannyProcess<TGreatGranny, TGreatGrannyReader, TGreatGrannyParameterSet, TGranny>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, TGranny>(
                        ProcessHelper.TryParse
                        )
                },
                network,
                this.externalReferences,
                out result
            );
    }
}
