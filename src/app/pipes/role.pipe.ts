import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'role'
})
export class RolePipe implements PipeTransform {

  transform(value: string): string {
    switch (value) {
      case '0':
        return 'Admin';
      case '1':
        return 'Moderator';
      case '2':
        return 'Organizer';
      case '3':
        return 'User';
      default:
        return 'Unknown role';
    }
  }
}
