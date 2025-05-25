using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class InstantiatesClassReader : IInstantiatesClassReader
    {
        private readonly IExpressionReader expressionReader;
        private readonly IMirrorSet mirrors;
        private readonly IAggregateParser aggregateParser;
        private static readonly IGreatGrannyProcess<IInstantiatesClass> target = new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IInstantiatesClass>(
            ProcessHelper.TryParse
        );

        public InstantiatesClassReader(
            IExpressionReader expressionReader, 
            IMirrorSet mirrors,
            IAggregateParser aggregateParser
        )
        {
            AssertionConcern.AssertArgumentNotNull(expressionReader, nameof(expressionReader));
            AssertionConcern.AssertArgumentNotNull(mirrors, nameof(mirrors));
            AssertionConcern.AssertArgumentNotNull(aggregateParser, nameof(aggregateParser));

            this.expressionReader = expressionReader;
            this.mirrors = mirrors;
            this.aggregateParser = aggregateParser;
        }

        public bool TryCreateGreatGrannies(
            IInstantiatesClassParameterSet parameters, 
            Network network, 
            IMirrorSet mirrors, 
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
                                () => InstantiatesClassReader.CreateSubordinationParameterSet(mirrors, parameters),
                                (g, r) => {
                                    if (coreBResult = g.TryGetValueUnitGrannyByTypeId(mirrors.DirectObject.Id, out IUnit vuResult))
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


        private static ExpressionParameterSet CreateSubordinationParameterSet(IMirrorSet mirrors, IInstantiatesClassParameterSet parameters) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithValueAndType(
                        mirrors.Instantiates,
                        mirrors.Unit
                    ),
                    UnitParameterSet.CreateWithValueAndType(
                        parameters.Class,
                        mirrors.DirectObject
                    )
                }
            );

        public bool TryParse(Network network, IInstantiatesClassParameterSet parameters, out IInstantiatesClass result) =>
            this.aggregateParser.TryParse<InstantiatesClass, IInstantiatesClass, IInstantiatesClassParameterSet>(
                parameters.Granny,
                this,
                parameters,
                network,
                this.mirrors,
                out result
            );
    }
}
