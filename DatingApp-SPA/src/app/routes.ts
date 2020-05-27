import {Routes} from '@angular/router';
import {HomeComponent} from './home/home.component';
import { MemberListComponent } from './member-list/member-list.component';
import { MessagesComponent } from './Messages/Messages.component';
import { LIstsComponent } from './LIsts/LIsts.component';
import { AuthGuard } from './_guards/auth.guard';

export const appRoutes: Routes = [
{ path: '', component: HomeComponent},
{
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
        { path: 'members', component: MemberListComponent, canActivate: [AuthGuard]},
        { path: 'messages', component: MessagesComponent},
        { path: 'lists', component: LIstsComponent},
     ]
},
{ path: '**', redirectTo: '', pathMatch: 'full'},
];
// For address link security when loggedout
