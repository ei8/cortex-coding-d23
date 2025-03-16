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
    >
        where TGreatGranny : IGranny
        where TGreatGrannyParameterSet : class, IDeductiveParameterSet
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

        private IEnumerable<IGreatGrannyInfo<TResult>> CreateGreatGrannies(
            TGreatGrannyReader reader,
            IExpressionReader expressionReader,
            TParameterSet parameters,
            IExternalReferenceSet externalReferences
        ) =>
            new IGreatGrannyInfo<TResult>[]
            {
                new IndependentGreatGrannyInfo<TGreatGranny, TGreatGrannyReader, TGreatGrannyParameterSet, TResult>(
                    reader,
                    () => this.CreateGreatGrannyParameterSet(parameters),
                    (g, r) => r.TypedGreatGranny = g
                    ),
                new DependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, TResult>(
                    expressionReader,
                    (g) => this.CreateExpressionParameterSet(externalReferences, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                    )
            };

        protected abstract TGreatGrannyParameterSet CreateGreatGrannyParameterSet(TParameterSet parameters);

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

        protected abstract ExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences, 
            TParameterSet parameters, 
            Neuron greatGranny
        );

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
