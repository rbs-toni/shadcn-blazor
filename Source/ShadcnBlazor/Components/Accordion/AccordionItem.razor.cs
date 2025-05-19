using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadcnBlazor;
public partial class AccordionItem
{
    readonly InternalAccordionItemContext _context;
    public AccordionItem()
    {
        Id = Identifier.NewId();
        _context = new(this);
    }
}
