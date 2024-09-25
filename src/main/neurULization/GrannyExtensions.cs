using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Selectors;
using Nancy.TinyIoc;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public static class GrannyExtensions
    {
        internal static void TryParseCore<TGranny>(
            this TGranny granny,
            Ensemble ensemble,
            IEnumerable<Guid> selection,
            LevelParser[] levelParsers,
            Action<Neuron> resultProcessor,
            ref TGranny result,
            bool throwExceptionOnRedundantData = true
        )
            where TGranny : IGranny
        {
            foreach (var levelParser in levelParsers)
                selection = levelParser.Evaluate(ensemble, selection);

            if (selection.Any())
            {
                if (throwExceptionOnRedundantData)
                    AssertionConcern.AssertStateTrue(
                        selection.Count() == 1,
                        $"Redundant items encountered while parsing ensemble: {string.Join(", ", selection)}"
                        );

                if (ensemble.TryGetById(selection.Single(), out Neuron ensembleResult))
                {
                    resultProcessor(ensembleResult);
                    result = granny;
                }
            }
        }

        public static void AddWriteProcessors(this TinyIoCContainer container)
        {
            container.Register<
                Processors.Writers.IExpressionProcessor, 
                Processors.Writers.ExpressionProcessor
            >();
            container.Register<
                Processors.Writers.IInstantiatesClassProcessor, 
                Processors.Writers.InstantiatesClassProcessor
            >();
            container.Register<
                Processors.Writers.IPropertyAssignmentProcessor, 
                Processors.Writers.PropertyAssignmentProcessor
            >();
            container.Register<
                Processors.Writers.IPropertyAssociationProcessor, 
                Processors.Writers.PropertyAssociationProcessor
            >();
            container.Register<
                Processors.Writers.IPropertyValueExpressionProcessor, 
                Processors.Writers.PropertyValueExpressionProcessor
            >();
            container.Register<
                Processors.Writers.IUnitProcessor, 
                Processors.Writers.UnitProcessor
            >();
            container.Register<
                Processors.Writers.IValueProcessor, 
                Processors.Writers.ValueProcessor
            >();
            container.Register<
                Processors.Writers.IValueExpressionProcessor, 
                Processors.Writers.ValueExpressionProcessor
            >();
            container.Register<
                Processors.Writers.IInstanceProcessor, 
                Processors.Writers.InstanceProcessor
            >();
        }

        public static void AddReadProcessors(this TinyIoCContainer container)
        {
            container.Register<
                Processors.Readers.Inductive.IExpressionProcessor,
                Processors.Readers.Inductive.ExpressionProcessor
            >();
            container.Register<
                Processors.Readers.Inductive.IInstantiatesClassProcessor,
                Processors.Readers.Inductive.InstantiatesClassProcessor
            >();
            container.Register<
                Processors.Readers.Inductive.IPropertyAssignmentProcessor,
                Processors.Readers.Inductive.PropertyAssignmentProcessor
                >();
            container.Register<
                Processors.Readers.Inductive.IPropertyAssociationProcessor,
                Processors.Readers.Inductive.PropertyAssociationProcessor
                >();
            container.Register<
               Processors.Readers.Inductive.IPropertyValueExpressionProcessor,
               Processors.Readers.Inductive.PropertyValueExpressionProcessor
            >();
            container.Register<
                Processors.Readers.Inductive.IUnitProcessor,
                Processors.Readers.Inductive.UnitProcessor
            >();
            container.Register<
                Processors.Readers.Inductive.IValueProcessor,
                Processors.Readers.Inductive.ValueProcessor
            >();
            container.Register<
                Processors.Readers.Inductive.IValueExpressionProcessor,
                Processors.Readers.Inductive.ValueExpressionProcessor
            >();
            container.Register<
                Processors.Readers.Inductive.IInstanceProcessor,
                Processors.Readers.Inductive.InstanceProcessor
            >();
        }
    }
}
