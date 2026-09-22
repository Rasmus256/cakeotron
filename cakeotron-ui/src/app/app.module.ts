import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { AppComponent } from './app.component';

@NgModule({
  declarations: [],
  imports: [BrowserModule, HttpClientModule, AppComponent],
  bootstrap: [AppComponent]
})
export class AppModule {}
