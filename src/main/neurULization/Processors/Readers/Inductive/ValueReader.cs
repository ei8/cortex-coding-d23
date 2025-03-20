using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class ValueReader : IValueReader
    {
        private readonly IInstantiatesClassReader instantiatesClassReader;
        private readonly IExpressionReader expressionReader;
        private readonly IExternalReferenceSet externalReferences;
        private readonly IAggregateParser aggregateParser;

        public ValueReader(
            IInstantiatesClassReader instantiatesClassReader, 
            IExpressionReader expressionReader,
            IExternalReferenceSet externalReferences,
            IAggregateParser aggregateParser
        )
        {
            AssertionConcern.AssertArgumentNotNull(instantiatesClassReader, nameof(instantiatesClassReader));
            AssertionConcern.AssertArgumentNotNull(expressionReader, nameof(expressionReader));
            AssertionConcern.AssertArgumentNotNull(externalReferences, nameof(externalReferences));
            AssertionConcern.AssertArgumentNotNull(aggregateParser, nameof(aggregateParser));

            this.instantiatesClassReader = instantiatesClassReader;
            this.expressionReader = expressionReader;
            this.externalReferences = externalReferences;
            this.aggregateParser = aggregateParser;
        }

        private static IGreatGrannyInfoSuperset<IInstanceValue> CreateGreatGrannies(
            IInstantiatesClassReader instantiatesClassReader,
            IExpressionReader expressionReader,
            IValueParameterSet parameters,
            Network network,
            IExternalReferenceSet externalReferences
        ) =>
            GreatGrannyInfoSuperset<IInstanceValue>.Create(
                new GreatGrannyInfoSet<IInstanceValue>[] {
                    ProcessHelper.CreateGreatGrannyCandidateSet(
                        network,
                        parameters.Granny,
                        gc => new InductiveIndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IInstanceValue>(
                            gc,
                            expressionReader,
                            () => ValueReader.CreateExpressionParameterSet(externalReferences, parameters, gc),
                            (g, r) => r.Expression = g
                        ),
                        new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IInstanceValue>(
                            ProcessHelper.TryParse
                        )
                    ),
                    new GreatGrannyInfoSet<IInstanceValue>(
                        new IGreatGrannyInfo<IInstanceValue>[]
                        {
                            new DependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IInstanceValue>(
                                instantiatesClassReader,
                                g => ValueReader.CreateInstantiatesClassParameterSet(
                                    parameters,
                                    ((IExpression) g).Units.GetValueUnitGranniesByTypeId(externalReferences.Unit.Id).Single().Value
                                    ),
                                (g, r) => r.TypedGreatGranny = g
                            )
                        },
                        new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IInstanceValue>(
                            ProcessHelper.TryParse
                        )
                    )
                }
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            IValueParameterSet parameters,
            Neuron unitGranny
        ) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithGrannyAndType(
                        unitGranny,
                        externalReferences.Unit
                    ),
                    UnitParameterSet.CreateWithGrannyAndType(
                        unitGranny,
                        externalReferences.NominalSubject
                    )
                }
            );

        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(
            IValueParameterSet parameters,
            Neuron grannyCandidate
            ) =>
            new InstantiatesClassParameterSet(
                grannyCandidate,
                parameters.Class
            );

        public bool TryParse(Network network, IValueParameterSet parameters, out IValue result)
        {
            var bResult = false;
            result = null;

            if (parameters.Class != null)
            {
                if (this.aggregateParser.TryParse<InstanceValue, IInstanceValue>(
                        parameters.Granny,
                        ValueReader.CreateGreatGrannies(
                            this.instantiatesClassReader, 
                            this.expressionReader,
                            parameters, 
                            network,
                            this.externalReferences
                        ),
                        network,
                        out IInstanceValue tempIV
                    )
                )
                {
                    bResult = true;
                    result = tempIV;
                }
            }
            else
            {
                bResult = true;
                result = new Value();
                result.Neuron = parameters.Granny;
            }

            return bResult;
        }
    }
}
