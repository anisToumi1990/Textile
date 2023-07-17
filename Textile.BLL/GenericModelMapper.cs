using System;
using System.Collections.Generic;
using System.Reflection;

namespace Textile.BLL
{

    public class GenericModelMapper
    {
        public static TDestinationType GetModel<TDestinationType, TSourceType>(TSourceType sourceObject)
        {
            object destinationObject = Activator.CreateInstance(typeof(TDestinationType));
            if (sourceObject != null)
            {
                foreach (PropertyInfo sourcePInfo in typeof(TSourceType).GetProperties())
                {
                    PropertyInfo destinationPInfo = typeof(TDestinationType).GetProperty(sourcePInfo.Name);
                    if (destinationPInfo != null)
                    {
                        try
                        {
                            destinationPInfo.SetValue(destinationObject, sourcePInfo.GetValue(sourceObject));
                        }
                        catch
                        {
                            //Last try --> conversion 
                            try
                            {
                                destinationPInfo.SetValue(destinationObject, Convert.ChangeType(sourcePInfo.GetValue(sourceObject), destinationPInfo.PropertyType));
                            }
                            catch
                            {
                                //don nothing when an exception is raised at this level
                                //continue with the rest of the properties  
                            }
                        }
                    }
                }
            }
            return ((TDestinationType)Convert.ChangeType(destinationObject, typeof(TDestinationType)));
        }
        public static List<TDestinationType> GetModelList<TDestinationType, TSourceType>(List<TSourceType> sourceObject)
        {
            List<TDestinationType> destinationType = new List<TDestinationType>();
            foreach (var item in sourceObject)
            {
                destinationType.Add(GetModel<TDestinationType, TSourceType>(item));
            }
            return destinationType;
        }
    }
}

