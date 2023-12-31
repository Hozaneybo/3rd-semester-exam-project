import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePage } from './home.page';
import {FeaturesComponent} from "./components/features/features.component";

const routes: Routes = [
  {
    path: '',
    component: HomePage,
    children: [
      {path: 'features', component: FeaturesComponent}
    ]
  },

  ];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomePageRoutingModule {}
