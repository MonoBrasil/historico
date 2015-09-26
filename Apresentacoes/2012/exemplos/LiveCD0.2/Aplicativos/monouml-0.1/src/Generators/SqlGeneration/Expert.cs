/*
MonoUML.Widgets - A library for representing the Widget elements
Copyright (C) 2005  Miguel Huerta

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
using ES = ExpertCoder.ExpertSystem;
		
namespace MonoUML.Generators.SqlGeneration
{
	[ES.ExpertSystemInformation(
		"UML to SQL",
		"Generates simple SQL code based on a UML model.",
		true)]
    public class Expert: ES.Expert                       
    {
        public Expert(ES.Environment env): base(env)
        {
			ES.Rule processDeserializedElementsRule = new ProcessDeserializedElementsRule();
            ES.Rule processClassRule = new ProcesClassRule();	
            ES.Rule processAttributeRule = new ProcessAttributeRule();		
            ES.Rule serializeClassRule = new SerializeClassRule();
            ES.Rule navigateClassRule = new NavigateClassRule();
            ES.Rule navigationPackageRule = new NavigatePackageRule();

            base.AddRule(processDeserializedElementsRule);
            base.AddRule(navigationPackageRule);
            base.AddRule(navigateClassRule); 
            base.AddRule(processClassRule);
            base.AddRule(processAttributeRule);
            base.AddRule(serializeClassRule);
            
            //Precedencia
            	
            base.AddPrecedence(new ES.Precedence(
            	processClassRule, navigateClassRule, ES.PrecedenceKind.Precedence, ES.CouplingKind.Independent));
            	
			 base.AddPrecedence(new ES.Precedence(
				navigateClassRule, serializeClassRule, ES.PrecedenceKind.Precedence, ES.CouplingKind.Independent));
        }
    }
}
