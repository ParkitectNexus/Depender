using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Depender.Types.Shops
{

    public class WerableProductInstance : WearableProduct
    {

        public WerableProductInstance()
        {

        }

        public override void Initialize()
        {
            this.gameObject.SetActive(true);

            base.Initialize();
        }
    }
}
