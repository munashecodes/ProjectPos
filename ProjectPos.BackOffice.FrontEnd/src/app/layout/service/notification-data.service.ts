import { Injectable } from '@angular/core';

interface Notification{
    purchaseOrder: number
    receivingOrder: number
}
@Injectable({
  providedIn: 'root'
})
export class NotificationDataService {

  public notification: Notification = {} as Notification

  constructor() { }
}
