import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';
import { FormsModule } from '@angular/forms';
import { HomePage } from './home.page';

import { HomePageRoutingModule } from './home-routing.module';
import { CompanyComponent } from './components/company/company.component';
import { CustomersComponent } from './components/customers/customers.component';
import { FeaturesComponent } from './components/features/features.component';
import { PricingComponent } from './components/pricing/pricing.component';
import { SupportComponent } from './components/support/support.component';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    HomePageRoutingModule
  ],
  declarations: [HomePage,
                CompanyComponent,
                CustomersComponent,
                FeaturesComponent,
                PricingComponent,
                SupportComponent]
})
export class HomePageModule {}
