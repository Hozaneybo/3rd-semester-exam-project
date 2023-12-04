import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import {FeaturesComponent} from "./home/components/features/features.component";
import {CompanyComponent} from "./home/components/company/company.component";
import {HomePage} from "./home/home.page";
import {CustomersComponent} from "./home/components/customers/customers.component";
import {PricingComponent} from "./home/components/pricing/pricing.component";
import {SupportComponent} from "./home/components/support/support.component";

const routes: Routes = [
  {
    path: 'home',
    loadChildren: () => import('./home/home.module').then( m => m.HomePageModule)
  },
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  { path: 'features', component: FeaturesComponent },
  { path: 'company', component: CompanyComponent },
  { path: 'customers', component: CustomersComponent },
  { path: 'pricing', component: PricingComponent },
  { path: 'support', component: SupportComponent },

  {
    path: 'admin',
    loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule)
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
