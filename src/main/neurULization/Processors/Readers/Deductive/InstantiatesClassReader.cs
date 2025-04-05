using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class InstantiatesClassReader : IInstantiatesClassReader
    {
        private readonly IExpressionReader expressionReader;
        private readonly IExternalReferenceSet externalReferences;

        public InstantiatesClassReader(IExpressionReader expressionReader, IExternalReferenceSet externalReferences)
        {
            this.expressionReader = expressionReader;
            this.externalReferences = externalReferences;
        }

        public bool TryCreateGreatGrannies(
            IInstantiatesClassParameterSet parameters,
            Network network,
            IExternalReferenceSet externalReferences,
            out IEnumerable<IGreatGrannyInfo<IInstantiatesClass>> result
        ) => this.TryCreateGreatGranniesCore(
            delegate (out bool bResult) {
            bResult = true;
            var coreBResult = true;
            var coreResult = new IGreatGrannyInfo<IInstantiatesClass>[]
                {
                    new IndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IInstantiatesClass>(
                        expressionReader,
                        () => InstantiatesClassReader.CreateSubordinationParameterSet(externalReferences, parameters),
                        (g, r) => {
                            if (coreBResult = g.TryGetValueUnitGrannyByTypeId(externalReferences.DirectObject.Id, out IUnit vuResult))
                                r.Class = vuResult;
                        }
                    )
                };
                bResult = coreBResult;
                return coreResult;
            },
            out result
        );

        public IEnumerable<IGrannyQuery> GetQueries(Network network, IInstantiatesClassParameterSet parameters) =>
            this.expressionReader.GetQueries(network,
                InstantiatesClassReader.CreateSubordinationParameterSet(this.externalReferences, parameters)
            );

        private static ExpressionParameterSet CreateSubordinationParameterSet(IExternalReferenceSet externalReferences, IInstantiatesClassParameterSet parameters)
        {
            return new ExpressionParameterSet(
                new[]
                {
                    new UnitParameterSet(
                        externalReferences.Instantiates,
                        externalReferences.Unit
                    ),
                    new UnitParameterSet(
                        parameters.Class,
                        externalReferences.DirectObject
                    )
                }
            );
        }

        public bool TryParse(Network network, IInstantiatesClassParameterSet parameters, out IInstantiatesClass result) =>
            this.TryParseAggregate(
                () => new InstantiatesClass(),
                parameters,
                new IGreatGrannyProcess<IInstantiatesClass>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IInstantiatesClass>(
                        ProcessHelper.TryParse
                    )
                },
                network,
                this.externalReferences,
                out result
            );
    }
}
