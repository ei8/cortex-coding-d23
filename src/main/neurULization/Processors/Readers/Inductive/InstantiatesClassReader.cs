using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class InstantiatesClassReader : IInstantiatesClassReader
    {
        private readonly IExpressionReader expressionReader;
        private readonly IExternalReferenceSet externalReferences;
        private readonly IAggregateParser aggregateParser;
        private static readonly IGreatGrannyProcess<IInstantiatesClass> target = new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IInstantiatesClass>(
            ProcessHelper.TryParse
        );

        public InstantiatesClassReader(
            IExpressionReader expressionReader, 
            IExternalReferenceSet externalReferences,
            IAggregateParser aggregateParser
        )
        {
            AssertionConcern.AssertArgumentNotNull(expressionReader, nameof(expressionReader));
            AssertionConcern.AssertArgumentNotNull(externalReferences, nameof(externalReferences));
            AssertionConcern.AssertArgumentNotNull(aggregateParser, nameof(aggregateParser));

            this.expressionReader = expressionReader;
            this.externalReferences = externalReferences;
            this.aggregateParser = aggregateParser;
        }

        public bool TryCreateGreatGrannies(
            IInstantiatesClassParameterSet parameters, 
            Network network, 
            IExternalReferenceSet externalReferences, 
            out IGreatGrannyInfoSuperset<IInstantiatesClass> result
        ) => this.TryCreateGreatGranniesCore(
            delegate (out bool bResult) {
                bResult = true;
                var coreBResult = true;
                var coreResult = GreatGrannyInfoSuperset<IInstantiatesClass>.Create(
                    new GreatGrannyInfoSet<IInstantiatesClass>(
                       new IGreatGrannyInfo<IInstantiatesClass>[]
                       {
                            new InductiveIndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IInstantiatesClass>(
                                parameters.Granny,
                                expressionReader,
                                () => InstantiatesClassReader.CreateSubordinationParameterSet(externalReferences, parameters),
                                (g, r) => {
                                    if (coreBResult = g.TryGetValueUnitGrannyByTypeId(externalReferences.DirectObject.Id, out IUnit vuResult))
                                        r.Class = vuResult;
                                }
                            )
                       },
                    InstantiatesClassReader.target
                ));
                bResult = coreBResult;
                return coreResult;
            },
            out result
        );


        private static ExpressionParameterSet CreateSubordinationParameterSet(IExternalReferenceSet externalReferences, IInstantiatesClassParameterSet parameters) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithValueAndType(
                        externalReferences.Instantiates,
                        externalReferences.Unit
                    ),
                    UnitParameterSet.CreateWithValueAndType(
                        parameters.Class,
                        externalReferences.DirectObject
                    )
                }
            );

        public bool TryParse(Network network, IInstantiatesClassParameterSet parameters, out IInstantiatesClass result) =>
            this.aggregateParser.TryParse<InstantiatesClass, IInstantiatesClass, IInstantiatesClassParameterSet>(
                parameters.Granny,
                this,
                parameters,
                network,
                this.externalReferences,
                out result
            );
    }
}
