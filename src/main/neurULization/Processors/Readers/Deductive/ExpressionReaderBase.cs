using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public abstract class ExpressionReaderBase<
        TGreatGranny, 
        TGreatGrannyParameterSet, 
        TGreatGrannyReader, 
        TResult,
        TParameterSet,
        TResultDerived
    > : ExpressionProcessorBase<
        TGreatGranny,
        TGreatGrannyParameterSet, 
        TGreatGrannyReader,
        TResult,
        TParameterSet
    >
        where TGreatGranny : IGranny
        where TGreatGrannyParameterSet : IDeductiveParameterSet
        where TGreatGrannyReader : IGrannyReader<TGreatGranny, TGreatGrannyParameterSet>
        where TResult : IExpressionGranny, ILesserGranny<TGreatGranny>
        where TParameterSet : IDeductiveParameterSet
        where TResultDerived : TResult, new()
    {
        private readonly TGreatGrannyReader greatGrannyReader;
        private readonly IExpressionReader expressionReader;
        private readonly IExternalReferenceSet externalReferences;

        protected ExpressionReaderBase(
            TGreatGrannyReader greatGrannyReader,
            IExpressionReader expressionReader,
            IExternalReferenceSet externalReferences
        )
        {
            this.greatGrannyReader = greatGrannyReader;
            this.expressionReader = expressionReader;
            this.externalReferences = externalReferences;
        }

        public IEnumerable<IGrannyQuery> GetQueries(Network network, TParameterSet parameters) =>
            new IGrannyQuery[] {
                new GreatGrannyQuery<TGreatGranny, TGreatGrannyReader, TGreatGrannyParameterSet>(
                    this.greatGrannyReader,
                    (n) => this.CreateGreatGrannyParameterSet(parameters)
                ),
                new GreatGrannyQuery<IExpression, IExpressionReader, IExpressionParameterSet>(
                    this.expressionReader,
                    (n) => this.CreateExpressionParameterSet(this.externalReferences, parameters, n.Last().Neuron)
                )
            };

        public bool TryParse(Network network, TParameterSet parameters, out TResult result) =>
            new TResultDerived().AggregateTryParse(
                this.CreateGreatGrannies(
                    this.greatGrannyReader,
                    this.expressionReader,
                    parameters,
                    this.externalReferences
                ),
                new IGreatGrannyProcess<TResult>[]
                {
                    new GreatGrannyProcess<TGreatGranny, TGreatGrannyReader, TGreatGrannyParameterSet, TResult>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, TResult>(
                        ProcessHelper.TryParse
                        )
                },
                network,
                out result
            );
    }
}
